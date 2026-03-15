using DroneComponentsInventory.Models;

namespace DroneComponentsInventory.Services
{
    public class BuildComponents
    {
        public FrameComponent? Frame { get; set; }
        public MotorsComponent? Motor { get; set; }
        public PropellersComponent? Propeller { get; set; }
        public ESCComponent? Esc { get; set; }
        public BatteryComponent? Battery { get; set; }
        public FCComponent? Fc { get; set; }
        public FPVCameraComponent? Camera { get; set; }
        public VideoTransmitterComponent? Vtx { get; set; }
        public VideoAntennaComponent? VideoAntenna { get; set; }
        public ReceiverComponent? Receiver { get; set; }
        public ReceiverAntennaComponent? ReceiverAntenna { get; set; }
        public RadioControllerComponent? RadioController { get; set; }
        public FPVGogglesComponent? FpvGoggles { get; set; }
    }

    public class CompatibilityCheckService
    {
        public List<CompatibilityResult> RunAllChecks(BuildComponents c)
        {
            return
            [
                Check01_MotorMountPattern(c),
                Check02_PropClearance(c),
                Check03_StackMounting(c),
                Check04_BatteryVoltageFamily(c),
                Check05_EscCurrentRating(c),
                Check06_ThrustToWeight(c),
                Check07_PropMotorMatch(c),
                Check08_BatteryCRating(c),
                Check09_FcFirmwareProtocol(c),
                Check10_ReceiverRadioProtocol(c),
                Check11_ReceiverAntenna(c),
                Check12_VideoSystem(c),
                Check13_VtxAntennaConnector(c),
                Check14_VtxPowerHeat(c),
                Check15_FcVoltageInput(c),
                Check16_TotalWeight(c),
                Check17_EstFlightTime(c)
            ];
        }

        // #1 Frame -> Motor mounting pattern
        private static CompatibilityResult Check01_MotorMountPattern(BuildComponents c)
        {
            var r = new CompatibilityResult { CheckNumber = 1, Name = "Motor Mount Pattern", Priority = 5 };

            if (c.Frame == null || c.Motor == null)
            { r.Status = "unchecked"; r.Message = "Frame or motors not selected"; return r; }

            if (string.IsNullOrWhiteSpace(c.Frame.MotorMountPattern) || string.IsNullOrWhiteSpace(c.Motor.MountPattern))
            { r.Status = "unchecked"; r.Message = "Missing mount pattern data"; return r; }

            if (c.Frame.MotorMountPattern.Contains(c.Motor.MountPattern, StringComparison.OrdinalIgnoreCase))
            { r.Status = "ok"; r.Message = $"Motor mount pattern matches ({c.Motor.MountPattern})"; }
            else
            { r.Status = "critical"; r.Message = $"Cannot mount -- frame: {c.Frame.MotorMountPattern}, motor: {c.Motor.MountPattern}"; }

            return r;
        }

        // #2 Frame -> Propeller clearance / max size
        private static CompatibilityResult Check02_PropClearance(BuildComponents c)
        {
            var r = new CompatibilityResult { CheckNumber = 2, Name = "Propeller Size", Priority = 5 };

            if (c.Frame == null || c.Propeller == null)
            { r.Status = "unchecked"; r.Message = "Frame or propellers not selected"; return r; }

            if (!c.Frame.MaxPropInch.HasValue || !c.Propeller.DiameterMm.HasValue)
            { r.Status = "unchecked"; r.Message = "Missing size data"; return r; }

            double propInch = c.Propeller.DiameterMm.Value / 25.4;
            double maxProp = c.Frame.MaxPropInch.Value;
            const double toleranceInch = 0.05;

            if (propInch <= maxProp + toleranceInch)
            { r.Status = "ok"; r.Message = $"Propellers fit -- {propInch:F1}\" <= {maxProp:F1}\" max"; }
            else if (propInch <= maxProp + 0.35)
            { r.Status = "warning"; r.Message = $"Propellers close to limit -- {propInch:F1}\" vs {maxProp:F1}\" max"; }
            else
            { r.Status = "critical"; r.Message = $"Propellers too large -- {propInch:F1}\" > {maxProp:F1}\" max"; }

            return r;
        }

        // #3 Frame -> Stack / FC + ESC mounting
        private static CompatibilityResult Check03_StackMounting(BuildComponents c)
        {
            var r = new CompatibilityResult { CheckNumber = 3, Name = "Stack Mounting (FC+ESC)", Priority = 4 };

            if (c.Frame == null || (c.Fc == null && c.Esc == null))
            { r.Status = "unchecked"; r.Message = "Frame or FC/ESC not selected"; return r; }

            if (string.IsNullOrWhiteSpace(c.Frame.FcMountPattern))
            { r.Status = "unchecked"; r.Message = "Missing frame stack size data"; return r; }

            var issues = new List<string>();
            var warnings = new List<string>();
            string framePattern = c.Frame.FcMountPattern.ToLower();

            if (c.Fc != null)
            {
                if (c.Fc.MountPatternMm == null)
                    warnings.Add("FC mount pattern is not set");
                else
                {
                    string fcSize = c.Fc.MountPatternMm.Value.ToString();
                    if (!framePattern.Contains(fcSize))
                        issues.Add($"FC mount {c.Fc.MountPatternMm}mm does not fit frame ({c.Frame.FcMountPattern})");
                }
            }

            if (c.Esc != null)
            {
                if (c.Esc.MountPatternMm == null)
                    warnings.Add("ESC mount pattern is not set");
                else
                {
                    string escSize = c.Esc.MountPatternMm.Value.ToString();
                    if (!framePattern.Contains(escSize))
                        issues.Add($"ESC mount {c.Esc.MountPatternMm}mm does not fit frame ({c.Frame.FcMountPattern})");
                }
            }

            if (issues.Count > 0)
            { r.Status = "critical"; r.Message = string.Join("; ", issues); }
            else if (warnings.Count > 0)
            { r.Status = "warning"; r.Message = string.Join("; ", warnings); }
            else
            { r.Status = "ok"; r.Message = "FC/ESC stack fits the frame"; }

            return r;
        }

        // #4 Battery voltage family compatibility
        private static CompatibilityResult Check04_BatteryVoltageFamily(BuildComponents c)
        {
            var r = new CompatibilityResult { CheckNumber = 4, Name = "Battery Voltage", Priority = 5 };

            if (c.Battery == null)
            { r.Status = "unchecked"; r.Message = "Battery not selected"; return r; }

            if (!c.Battery.CellCountS.HasValue)
            { r.Status = "unchecked"; r.Message = "Missing battery cell count"; return r; }

            int batteryS = c.Battery.CellCountS.Value;
            var issues = new List<string>();
            var warnings = new List<string>();

            if (c.Motor != null)
            {
                if (c.Motor.RecommendedVoltageS == null)
                    warnings.Add("Motor recommended voltage (S) is not set");
                else if (c.Motor.RecommendedVoltageS != batteryS)
                    issues.Add($"Motor recommends {c.Motor.RecommendedVoltageS}S, but battery is {batteryS}S");
            }

            if (c.Esc != null)
            {
                if (c.Esc.VoltageInputS == null)
                    warnings.Add("ESC voltage input (S) is not set");
                else if (batteryS > c.Esc.VoltageInputS)
                    issues.Add($"ESC supports up to {c.Esc.VoltageInputS}S, but battery is {batteryS}S -- ESC will burn!");
            }

            if (issues.Any(i => i.Contains("burn")))
            { r.Status = "critical"; r.Message = string.Join("; ", issues); }
            else if (issues.Count > 0)
            { r.Status = "warning"; r.Message = string.Join("; ", issues); }
            else if (warnings.Count > 0)
            { r.Status = "warning"; r.Message = string.Join("; ", warnings); }
            else if (c.Motor != null || c.Esc != null)
            { r.Status = "ok"; r.Message = $"Battery {batteryS}S is compatible"; }
            else
            { r.Status = "unchecked"; r.Message = "No motors/ESC to verify voltage"; }

            return r;
        }

        // #5 ESC current rating vs. motor demands
        private static CompatibilityResult Check05_EscCurrentRating(BuildComponents c)
        {
            var r = new CompatibilityResult { CheckNumber = 5, Name = "ESC Current Rating", Priority = 5 };

            if (c.Esc == null || c.Motor == null)
            { r.Status = "unchecked"; r.Message = "ESC or motor not selected"; return r; }

            if (!c.Esc.ContinuousCurrentA.HasValue || !c.Motor.MaxThrustG.HasValue)
            { r.Status = "unchecked"; r.Message = "Missing current or thrust data"; return r; }

            int cellCount = c.Battery?.CellCountS ?? c.Motor.RecommendedVoltageS ?? 4;
            double estPeakCurrent = EstimateMotorPeakCurrent(c.Motor.MaxThrustG.Value, cellCount);
            double escCurrent = c.Esc.ContinuousCurrentA.Value;

            if (escCurrent >= estPeakCurrent * 1.3)
            { r.Status = "ok"; r.Message = $"ESC {escCurrent}A is sufficient (motor ~{estPeakCurrent:F0}A peak)"; }
            else if (escCurrent >= estPeakCurrent)
            { r.Status = "warning"; r.Message = $"ESC {escCurrent}A is close to limit (motor ~{estPeakCurrent:F0}A peak)"; }
            else
            { r.Status = "critical"; r.Message = $"ESC {escCurrent}A too weak -- motor can draw ~{estPeakCurrent:F0}A peak!"; }

            return r;
        }

        // #6 Thrust-to-weight ratio (AUW)
        private static CompatibilityResult Check06_ThrustToWeight(BuildComponents c)
        {
            var r = new CompatibilityResult { CheckNumber = 6, Name = "Thrust-to-Weight Ratio", Priority = 5 };

            if (c.Motor == null)
            { r.Status = "unchecked"; r.Message = "Motors not selected"; return r; }

            if (!c.Motor.MaxThrustG.HasValue)
            { r.Status = "unchecked"; r.Message = "Missing motor thrust data"; return r; }

            int motorCount = GetMotorCount(c.Frame);
            double totalThrust = c.Motor.MaxThrustG.Value * motorCount;
            double totalWeight = CalculateTotalWeight(c);

            if (totalWeight <= 0)
            { r.Status = "unchecked"; r.Message = "Missing weight data"; return r; }

            double ratio = totalThrust / totalWeight;

            if (ratio >= 4.0)
            { r.Status = "ok"; r.Message = $"Excellent thrust! Ratio {ratio:F1}:1 (thrust {totalThrust:F0}g, weight {totalWeight:F0}g)"; }
            else if (ratio >= 3.0)
            { r.Status = "ok"; r.Message = $"Good thrust. Ratio {ratio:F1}:1 (thrust {totalThrust:F0}g, weight {totalWeight:F0}g)"; }
            else if (ratio >= 2.0)
            { r.Status = "warning"; r.Message = $"Barely flyable. Ratio {ratio:F1}:1 (thrust {totalThrust:F0}g, weight {totalWeight:F0}g)"; }
            else
            { r.Status = "critical"; r.Message = $"Won't fly! Ratio {ratio:F1}:1 (thrust {totalThrust:F0}g, weight {totalWeight:F0}g)"; }

            return r;
        }

        // #7 Propeller <-> Motor recommendations
        private static CompatibilityResult Check07_PropMotorMatch(BuildComponents c)
        {
            var r = new CompatibilityResult { CheckNumber = 7, Name = "Propeller-Motor Match", Priority = 4 };

            if (c.Propeller == null || c.Motor == null)
            { r.Status = "unchecked"; r.Message = "Propellers or motors not selected"; return r; }

            var issues = new List<string>();
            var informational = new List<string>();

            if (!string.IsNullOrWhiteSpace(c.Propeller.RecommendedMotorSize))
            {
                if (!c.Motor.StatorSizeMm.HasValue)
                {
                    informational.Add("Motor stator size is not set");
                }
                else
                {
                    bool exactTextMatch = c.Propeller.RecommendedMotorSize.Contains(c.Motor.StatorSizeMm.Value.ToString());

                    if (!exactTextMatch)
                    {
                        int? motorClass = GetStatorDiameterClass(c.Motor.StatorSizeMm.Value);
                        var recommendedClasses = GetStatorDiameterClasses(c.Propeller.RecommendedMotorSize);

                        if (motorClass.HasValue && recommendedClasses.Count > 0)
                        {
                            int closestDiff = recommendedClasses.Min(x => Math.Abs(x - motorClass.Value));
                            if (closestDiff > 3)
                                issues.Add($"Propellers recommend motor {c.Propeller.RecommendedMotorSize}, but selected {c.Motor.StatorSizeMm}");
                        }
                        else
                        {
                            informational.Add("Motor size recommendation format could not be fully compared");
                        }
                    }
                }
            }

            if (c.Propeller.RecommendedMotorKv.HasValue)
            {
                if (!c.Motor.Kv.HasValue)
                {
                    informational.Add("Motor KV is not set");
                }
                else
                {
                    double kvDiff = Math.Abs(c.Propeller.RecommendedMotorKv.Value - c.Motor.Kv.Value);
                    double kvPercent = kvDiff / c.Propeller.RecommendedMotorKv.Value;
                    if (kvPercent > 0.35)
                        issues.Add($"KV mismatch -- propellers recommend ~{c.Propeller.RecommendedMotorKv}KV, motor is {c.Motor.Kv}KV");
                }
            }

            if (c.Motor.RecommendedPropInch.HasValue && c.Propeller.DiameterMm.HasValue)
            {
                double propInch = c.Propeller.DiameterMm.Value / 25.4;
                double diff = Math.Abs(c.Motor.RecommendedPropInch.Value - propInch);
                if (diff > 0.5)
                    issues.Add($"Motor recommends ~{c.Motor.RecommendedPropInch:F1}\" props, selected {propInch:F1}\"");
            }

            if (issues.Count > 0)
            { r.Status = "warning"; r.Message = string.Join("; ", issues); }
            else
            {
                r.Status = "ok";
                r.Message = informational.Count > 0
                    ? $"Propellers and motors are compatible ({string.Join("; ", informational)})"
                    : "Propellers match the motors";
            }

            return r;
        }

        // #8 Battery C-rating / burst vs. max current draw
        private static CompatibilityResult Check08_BatteryCRating(BuildComponents c)
        {
            var r = new CompatibilityResult { CheckNumber = 8, Name = "Battery C-Rating", Priority = 4 };

            if (c.Battery == null || c.Motor == null)
            { r.Status = "unchecked"; r.Message = "Battery or motors not selected"; return r; }

            if (!c.Battery.BurstRateC.HasValue || !c.Battery.CapacityMah.HasValue || !c.Motor.MaxThrustG.HasValue)
            { r.Status = "unchecked"; r.Message = "Missing C-rating or capacity data"; return r; }

            int cellCount = c.Battery.CellCountS ?? 4;
            double peakPerMotor = EstimateMotorPeakCurrent(c.Motor.MaxThrustG.Value, cellCount);
            int motorCount = GetMotorCount(c.Frame);
            double totalPeakCurrent = peakPerMotor * motorCount;
            double batteryMaxBurst = c.Battery.BurstRateC.Value * (c.Battery.CapacityMah.Value / 1000.0);

            if (batteryMaxBurst >= totalPeakCurrent * 1.2)
            { r.Status = "ok"; r.Message = $"Battery can handle it -- {batteryMaxBurst:F0}A burst >= {totalPeakCurrent:F0}A needed"; }
            else if (batteryMaxBurst >= totalPeakCurrent)
            { r.Status = "warning"; r.Message = $"Battery close to limit -- {batteryMaxBurst:F0}A burst vs {totalPeakCurrent:F0}A needed"; }
            else
            { r.Status = "critical"; r.Message = $"Battery too weak! {batteryMaxBurst:F0}A burst < {totalPeakCurrent:F0}A needed"; }

            return r;
        }

        // #9 FC firmware supports radio protocol
        private static CompatibilityResult Check09_FcFirmwareProtocol(BuildComponents c)
        {
            var r = new CompatibilityResult { CheckNumber = 9, Name = "FC Firmware / Radio Protocol", Priority = 4 };

            if (c.Fc == null || c.Receiver == null)
            { r.Status = "unchecked"; r.Message = "FC or receiver not selected"; return r; }

            if (string.IsNullOrWhiteSpace(c.Fc.FirmwareSupport) || string.IsNullOrWhiteSpace(c.Receiver.Protocol))
            { r.Status = "unchecked"; r.Message = "Missing firmware/protocol data"; return r; }

            string firmware = c.Fc.FirmwareSupport.ToLower();
            string protocol = c.Receiver.Protocol.ToLower();

            if (firmware.Contains("betaflight") || firmware.Contains("inav"))
            { r.Status = "ok"; r.Message = $"FC firmware ({c.Fc.FirmwareSupport}) supports {c.Receiver.Protocol} via UART"; }
            else
            { r.Status = "ok"; r.Message = $"FC firmware: {c.Fc.FirmwareSupport}, Protocol: {c.Receiver.Protocol} -- verify manually"; }

            return r;
        }

        // #10 Receiver <-> Radio Controller protocol
        private static CompatibilityResult Check10_ReceiverRadioProtocol(BuildComponents c)
        {
            var r = new CompatibilityResult { CheckNumber = 10, Name = "Receiver / Radio Protocol", Priority = 4 };

            if (c.Receiver == null || c.RadioController == null)
            { r.Status = "unchecked"; r.Message = "Receiver or radio controller not selected"; return r; }

            if (string.IsNullOrWhiteSpace(c.Receiver.Protocol) || string.IsNullOrWhiteSpace(c.RadioController.ProtocolsSupported))
            { r.Status = "unchecked"; r.Message = "Missing protocol data"; return r; }

            string rxProtocol = c.Receiver.Protocol.Trim().ToLower();
            string txProtocols = c.RadioController.ProtocolsSupported.ToLower();

            if (txProtocols.Contains(rxProtocol))
            { r.Status = "ok"; r.Message = $"Protocol matches -- {c.Receiver.Protocol}"; }
            else
            { r.Status = "critical"; r.Message = $"Protocol mismatch! Receiver: {c.Receiver.Protocol}, Radio: {c.RadioController.ProtocolsSupported}"; }

            return r;
        }

        // #11 Receiver antenna compatibility
        private static CompatibilityResult Check11_ReceiverAntenna(BuildComponents c)
        {
            var r = new CompatibilityResult { CheckNumber = 11, Name = "Receiver Antenna", Priority = 3 };

            if (c.Receiver == null || c.ReceiverAntenna == null)
            { r.Status = "unchecked"; r.Message = "Receiver or antenna not selected"; return r; }

            bool bandKnown = !string.IsNullOrWhiteSpace(c.ReceiverAntenna.FrequencyBand)
                          && !string.IsNullOrWhiteSpace(c.Receiver.FrequencyBand);
            bool bandMatches = bandKnown && c.ReceiverAntenna.FrequencyBand!
                .Equals(c.Receiver.FrequencyBand, StringComparison.OrdinalIgnoreCase);

            if (!string.IsNullOrWhiteSpace(c.ReceiverAntenna.CompatibleReceivers))
            {
                string compatible = c.ReceiverAntenna.CompatibleReceivers;
                string rxModel = c.Receiver.Model ?? string.Empty;

                if (!string.IsNullOrWhiteSpace(rxModel)
                    && (compatible.Contains(rxModel, StringComparison.OrdinalIgnoreCase)
                        || HasMeaningfulModelOverlap(compatible, rxModel)))
                {
                    r.Status = "ok";
                    r.Message = "Antenna is compatible with receiver";
                }
                else if (bandMatches)
                {
                    r.Status = "ok";
                    r.Message = $"Frequency band matches ({c.Receiver.FrequencyBand}) -- direct model match not found";
                }
                else if (bandKnown)
                {
                    r.Status = "warning";
                    r.Message = $"Receiver not listed in antenna compatibility and frequency band differs ({c.ReceiverAntenna.FrequencyBand} vs {c.Receiver.FrequencyBand}) -- verify manually";
                }
                else
                {
                    r.Status = "warning";
                    r.Message = "Receiver not listed in antenna compatibility -- verify manually";
                }
            }
            else
            {
                if (bandMatches)
                { r.Status = "ok"; r.Message = $"Frequency band matches ({c.Receiver.FrequencyBand})"; }
                else if (bandKnown)
                { r.Status = "warning"; r.Message = $"Frequency band may not match ({c.ReceiverAntenna.FrequencyBand} vs {c.Receiver.FrequencyBand})"; }
                else
                { r.Status = "unchecked"; r.Message = "Missing compatibility data"; }
            }

            return r;
        }

        // #12 Video system -- analog vs digital
        private static CompatibilityResult Check12_VideoSystem(BuildComponents c)
        {
            var r = new CompatibilityResult { CheckNumber = 12, Name = "Video System (Analog/Digital)", Priority = 4 };

            if (c.Camera == null && c.Vtx == null && c.FpvGoggles == null)
            { r.Status = "unchecked"; r.Message = "No video components selected"; return r; }

            var systems = new List<string>();
            if (!string.IsNullOrWhiteSpace(c.Camera?.TypeSystem)) systems.Add(c.Camera.TypeSystem.Trim());
            if (!string.IsNullOrWhiteSpace(c.Vtx?.Type)) systems.Add(c.Vtx.Type.Trim());
            if (!string.IsNullOrWhiteSpace(c.FpvGoggles?.VideoSystem)) systems.Add(c.FpvGoggles.VideoSystem.Trim());

            if (systems.Count < 2)
            { r.Status = "unchecked"; r.Message = "Not enough video components to check compatibility"; return r; }

            var normalized = systems.Select(NormalizeVideoSystem).ToList();

            if (normalized.Distinct().Count() == 1)
            { r.Status = "ok"; r.Message = $"Video system is unified -- {systems[0]}"; }
            else
            { r.Status = "critical"; r.Message = $"Video systems mismatch! No image in goggles. ({string.Join(", ", systems)})"; }

            return r;
        }

        // #13 VTX antenna connector
        private static CompatibilityResult Check13_VtxAntennaConnector(BuildComponents c)
        {
            var r = new CompatibilityResult { CheckNumber = 13, Name = "VTX Antenna Connector", Priority = 4 };

            if (c.Vtx == null || c.VideoAntenna == null)
            { r.Status = "unchecked"; r.Message = "VTX or video antenna not selected"; return r; }

            if (string.IsNullOrWhiteSpace(c.Vtx.AntennaConnector) || string.IsNullOrWhiteSpace(c.VideoAntenna.Connector))
            { r.Status = "unchecked"; r.Message = "Missing connector data"; return r; }

            if (c.Vtx.AntennaConnector.Equals(c.VideoAntenna.Connector, StringComparison.OrdinalIgnoreCase))
            { r.Status = "ok"; r.Message = $"Connector matches ({c.Vtx.AntennaConnector})"; }
            else
            { r.Status = "warning"; r.Message = $"Connector mismatch! VTX: {c.Vtx.AntennaConnector}, Antenna: {c.VideoAntenna.Connector} -- use an adapter pigtail or the VTX's included antenna"; }

            return r;
        }

        // #14 VTX power / heat dissipation
        private static CompatibilityResult Check14_VtxPowerHeat(BuildComponents c)
        {
            var r = new CompatibilityResult { CheckNumber = 14, Name = "VTX Power / Heat", Priority = 3 };

            if (c.Vtx == null)
            { r.Status = "unchecked"; r.Message = "VTX not selected"; return r; }

            if (!c.Vtx.MaxPowerMw.HasValue)
            { r.Status = "unchecked"; r.Message = "Missing VTX power data"; return r; }

            if (c.Vtx.MaxPowerMw <= 600)
            { r.Status = "ok"; r.Message = $"VTX power {c.Vtx.MaxPowerMw}mW -- acceptable for well-ventilated builds"; }
            else if (c.Vtx.MaxPowerMw <= 1200)
            { r.Status = "warning"; r.Message = $"VTX power {c.Vtx.MaxPowerMw}mW -- good ventilation recommended"; }
            else
            { r.Status = "warning"; r.Message = $"VTX power {c.Vtx.MaxPowerMw}mW -- high heat, active cooling needed"; }

            return r;
        }

        // #15 FC voltage input compatibility
        private static CompatibilityResult Check15_FcVoltageInput(BuildComponents c)
        {
            var r = new CompatibilityResult { CheckNumber = 15, Name = "FC Voltage Input", Priority = 3 };

            if (c.Fc == null || c.Battery == null)
            { r.Status = "unchecked"; r.Message = "FC or battery not selected"; return r; }

            if (string.IsNullOrWhiteSpace(c.Fc.VoltageInputS) || !c.Battery.CellCountS.HasValue)
            { r.Status = "unchecked"; r.Message = "Missing voltage data"; return r; }

            var (minS, maxS) = ParseVoltageRange(c.Fc.VoltageInputS);
            int batteryS = c.Battery.CellCountS.Value;

            if (minS == 0 && maxS == 0)
            { r.Status = "unchecked"; r.Message = $"Cannot parse FC voltage range ({c.Fc.VoltageInputS})"; return r; }

            if (batteryS >= minS && batteryS <= maxS)
            { r.Status = "ok"; r.Message = $"FC supports {batteryS}S (range: {minS}-{maxS}S)"; }
            else
            { r.Status = "critical"; r.Message = $"FC does not support {batteryS}S! Range: {minS}-{maxS}S"; }

            return r;
        }

        // #16 Total weight -- sub-250g or limits
        private static CompatibilityResult Check16_TotalWeight(BuildComponents c)
        {
            var r = new CompatibilityResult { CheckNumber = 16, Name = "Total Weight", Priority = 2 };

            var missingWeights = GetMissingWeightWarnings(c);
            double totalWeight = CalculateTotalWeight(c);

            if (totalWeight <= 0 && missingWeights.Count == 0)
            { r.Status = "unchecked"; r.Message = "Missing weight data"; return r; }

            if (missingWeights.Count > 0)
            {
                string missingList = string.Join("; ", missingWeights);
                if (totalWeight > 0 && totalWeight <= 250 && missingWeights.Count <= 1)
                {
                    r.Status = "ok";
                    r.Message = $"Estimated under 250g ({totalWeight:F0}g) with minor missing data ({missingList})";
                }
                else if (totalWeight > 0)
                {
                    r.Status = "warning";
                    r.Message = $"Partial weight {totalWeight:F0}g -- actual total will be higher ({missingList})";
                }
                else
                {
                    r.Status = "warning";
                    r.Message = $"Weight not set for: {missingList}";
                }
                return r;
            }

            if (totalWeight <= 250)
            { r.Status = "ok"; r.Message = $"Under 250g ({totalWeight:F0}g) -- no registration required in most countries"; }
            else if (totalWeight <= 500)
            { r.Status = "warning"; r.Message = $"Total weight {totalWeight:F0}g -- exceeds 250g limit"; }
            else
            { r.Status = "warning"; r.Message = $"Total weight {totalWeight:F0}g"; }

            return r;
        }

        // #17 Estimated flight time (bonus)
        private static CompatibilityResult Check17_EstFlightTime(BuildComponents c)
        {
            var r = new CompatibilityResult { CheckNumber = 17, Name = "Est. Flight Time", Priority = 2 };

            if (c.Battery == null || c.Motor == null)
            { r.Status = "unchecked"; r.Message = "Battery or motors not selected"; return r; }

            if (!c.Battery.CapacityMah.HasValue || !c.Battery.CellCountS.HasValue || !c.Motor.MaxThrustG.HasValue)
            { r.Status = "unchecked"; r.Message = "Missing capacity/thrust data"; return r; }

            double totalWeight = CalculateTotalWeight(c);
            if (totalWeight <= 0)
            { r.Status = "unchecked"; r.Message = "Missing weight data for flight time calculation"; return r; }

            int cellCount = c.Battery.CellCountS.Value;
            double voltage = cellCount * 3.7;
            int motorCount = GetMotorCount(c.Frame);

            // Hover: each motor produces thrust = totalWeight / motor count
            double hoverThrustPerMotor = totalWeight / motorCount;
            double hoverPowerPerMotor = hoverThrustPerMotor / 5.0; // ~5 g/W at hover
            double hoverCurrentPerMotor = hoverPowerPerMotor / voltage;
            double totalHoverCurrent = hoverCurrentPerMotor * motorCount;

            double usableCapacityAh = c.Battery.CapacityMah.Value / 1000.0 * 0.8; // 80% usable
            double flightTimeMinutes = (usableCapacityAh / totalHoverCurrent) * 60;

            r.Status = "ok";
            r.Message = $"~{flightTimeMinutes:F1} min (estimated hover time at 80% battery)";
            return r;
        }

        // --- Helper methods ---

        private static double EstimateMotorPeakCurrent(int maxThrustG, int cellCount)
        {
            double voltage = cellCount * 3.7;
            double powerW = maxThrustG / 2.5; // ~2.5 g/W at max thrust
            return powerW / voltage;
        }

        private static double CalculateTotalWeight(BuildComponents c)
        {
            double weight = 0;
            int knownParts = 0;
            int motorCount = GetMotorCount(c.Frame);
            int propellerCount = GetPropellerCount(c.Frame, c.Propeller);

            if (c.Frame?.WeightG != null) { weight += c.Frame.WeightG.Value; knownParts++; }
            if (c.Motor?.WeightG != null) { weight += c.Motor.WeightG.Value * motorCount; knownParts++; }
            if (c.Propeller?.WeightG != null) { weight += c.Propeller.WeightG.Value * propellerCount; knownParts++; }
            if (c.Esc?.WeightG != null) { weight += c.Esc.WeightG.Value; knownParts++; }
            if (c.Battery?.WeightG != null) { weight += c.Battery.WeightG.Value; knownParts++; }
            if (c.Fc?.WeightG != null) { weight += c.Fc.WeightG.Value; knownParts++; }
            if (c.Camera?.WeightG != null) { weight += c.Camera.WeightG.Value; knownParts++; }
            if (c.Vtx?.WeightG != null) { weight += c.Vtx.WeightG.Value; knownParts++; }
            if (c.VideoAntenna?.WeightG != null) { weight += c.VideoAntenna.WeightG.Value; knownParts++; }
            if (c.Receiver?.WeightG != null) { weight += c.Receiver.WeightG.Value; knownParts++; }
            if (c.ReceiverAntenna?.WeightG != null) { weight += c.ReceiverAntenna.WeightG.Value; knownParts++; }
            // RadioController and FPVGoggles are ground equipment -- not on drone

            if (knownParts >= 3) weight += 30; // wiring, screws, zip ties estimate

            return weight;
        }

        private static int GetMotorCount(FrameComponent? frame)
        {
            if (frame == null || string.IsNullOrWhiteSpace(frame.Geometry))
                return 4;

            string geometry = frame.Geometry.Trim().ToLowerInvariant();

            if (geometry.Contains("octo") || geometry.Contains("x8") || geometry.Contains("8x"))
                return 8;

            if (geometry.Contains("hex") || geometry.Contains("x6") || geometry.Contains("6x") || geometry.Contains("y6"))
                return 6;

            if (geometry.Contains("tri"))
                return 3;

            if (geometry.Contains("bi"))
                return 2;

            return 4; // True X, Deadcat, H, and unknown defaults to quad
        }

        private static int GetPropellerCount(FrameComponent? frame, PropellersComponent? propeller)
        {
            int motorCount = GetMotorCount(frame);

            if (frame == null && propeller?.IncludedQuantity is > 0)
                return propeller.IncludedQuantity.Value;

            return motorCount;
        }

        private static int? GetStatorDiameterClass(double statorSize)
        {
            string digits = ((int)Math.Round(statorSize)).ToString();
            if (digits.Length >= 4 && int.TryParse(digits[..2], out int fourDigitClass))
                return fourDigitClass;

            if (digits.Length >= 2 && int.TryParse(digits[..2], out int twoDigitClass))
                return twoDigitClass;

            return null;
        }

        private static List<int> GetStatorDiameterClasses(string motorSizeText)
        {
            var result = new List<int>();
            if (string.IsNullOrWhiteSpace(motorSizeText))
                return result;

            string current = string.Empty;
            foreach (char ch in motorSizeText)
            {
                if (char.IsDigit(ch))
                {
                    current += ch;
                }
                else if (current.Length > 0)
                {
                    AddStatorClassToken(current, result);
                    current = string.Empty;
                }
            }

            if (current.Length > 0)
                AddStatorClassToken(current, result);

            return result.Distinct().ToList();
        }

        private static void AddStatorClassToken(string digits, List<int> result)
        {
            if (digits.Length >= 4 && int.TryParse(digits[..2], out int cls4))
            {
                result.Add(cls4);
                return;
            }

            if (digits.Length >= 2 && int.TryParse(digits[..2], out int cls2))
                result.Add(cls2);
        }

        private static bool HasMeaningfulModelOverlap(string a, string b)
        {
            var aTokens = TokenizeModel(a);
            var bTokens = TokenizeModel(b);
            return aTokens.Intersect(bTokens).Any();
        }

        private static HashSet<string> TokenizeModel(string text)
        {
            var tokens = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (string.IsNullOrWhiteSpace(text))
                return tokens;

            string current = string.Empty;
            foreach (char ch in text)
            {
                if (char.IsLetterOrDigit(ch))
                {
                    current += char.ToLowerInvariant(ch);
                }
                else if (current.Length > 0)
                {
                    AddModelToken(current, tokens);
                    current = string.Empty;
                }
            }

            if (current.Length > 0)
                AddModelToken(current, tokens);

            return tokens;
        }

        private static void AddModelToken(string token, HashSet<string> tokens)
        {
            if (token.Length < 3)
                return;

            if (token is "receiver" or "expresslrs" or "nano")
                return;

            tokens.Add(token);
        }

        private static List<string> GetMissingWeightWarnings(BuildComponents c)
        {
            var missing = new List<string>();
            if (c.Frame != null && c.Frame.WeightG == null) missing.Add("Frame weight is not set");
            if (c.Motor != null && c.Motor.WeightG == null) missing.Add("Motor weight is not set");
            if (c.Propeller != null && c.Propeller.WeightG == null) missing.Add("Propeller weight is not set");
            if (c.Esc != null && c.Esc.WeightG == null) missing.Add("ESC weight is not set");
            if (c.Battery != null && c.Battery.WeightG == null) missing.Add("Battery weight is not set");
            if (c.Fc != null && c.Fc.WeightG == null) missing.Add("FC weight is not set");
            if (c.Camera != null && c.Camera.WeightG == null) missing.Add("Camera weight is not set");
            if (c.Vtx != null && c.Vtx.WeightG == null) missing.Add("VTX weight is not set");
            if (c.VideoAntenna != null && c.VideoAntenna.WeightG == null) missing.Add("Video antenna weight is not set");
            if (c.Receiver != null && c.Receiver.WeightG == null) missing.Add("Receiver weight is not set");
            if (c.ReceiverAntenna != null && c.ReceiverAntenna.WeightG == null) missing.Add("Receiver antenna weight is not set");
            return missing;
        }

        private static (int min, int max) ParseVoltageRange(string voltageStr)
        {
            var cleaned = voltageStr
                .Replace("S", "", StringComparison.OrdinalIgnoreCase)
                .Replace("s", "")
                .Trim();

            var parts = cleaned.Split(['-', '–', '~'], StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length >= 2
                && int.TryParse(parts[0].Trim(), out int min)
                && int.TryParse(parts[1].Trim(), out int max))
                return (min, max);

            if (parts.Length == 1 && int.TryParse(parts[0].Trim(), out int single))
                return (single, single);

            return (0, 0);
        }

        private static string NormalizeVideoSystem(string system)
        {
            system = system.ToLower().Trim();
            if (system.Contains("analog")) return "analog";
            if (system.Contains("dji") || system.Contains("o3") || system.Contains("vista")) return "dji";
            if (system.Contains("walksnail") || system.Contains("avatar")) return "walksnail";
            if (system.Contains("hdzero") || system.Contains("hd zero")) return "hdzero";
            return system;
        }
    }
}
