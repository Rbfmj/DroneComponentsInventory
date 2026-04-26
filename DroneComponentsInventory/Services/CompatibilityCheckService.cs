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

        // #1 Rāmis -> Motoru stiprinājumu raksts
        private static CompatibilityResult Check01_MotorMountPattern(BuildComponents c)
        {
            var r = new CompatibilityResult { CheckNumber = 1, Name = "Motoru stiprinājumu raksts", Priority = 5 };

            if (c.Frame == null || c.Motor == null)
            { r.Status = "unchecked"; r.Message = "Rāmis vai motori nav izvēlēti"; return r; }

            if (string.IsNullOrWhiteSpace(c.Frame.MotorMountPattern) || string.IsNullOrWhiteSpace(c.Motor.MountPattern))
            { r.Status = "unchecked"; r.Message = "Trūkst datu par stiprinājumu rakstu"; return r; }

            if (c.Frame.MotorMountPattern.Contains(c.Motor.MountPattern, StringComparison.OrdinalIgnoreCase))
            { r.Status = "ok"; r.Message = $"Motoru stiprinājumu raksts sakrīt ({c.Motor.MountPattern})"; }
            else
            { r.Status = "critical"; r.Message = $"Nevar uzstādīt -- rāmis: {c.Frame.MotorMountPattern}, motors: {c.Motor.MountPattern}"; }

            return r;
        }

        // #2 Rāmis -> Propelleru izmērs
        private static CompatibilityResult Check02_PropClearance(BuildComponents c)
        {
            var r = new CompatibilityResult { CheckNumber = 2, Name = "Propelleru izmērs", Priority = 5 };

            if (c.Frame == null || c.Propeller == null)
            { r.Status = "unchecked"; r.Message = "Rāmis vai propelleri nav izvēlēti"; return r; }

            if (!c.Frame.MaxPropInch.HasValue || !c.Propeller.DiameterMm.HasValue)
            { r.Status = "unchecked"; r.Message = "Trūkst izmēra datu"; return r; }

            double propInch = c.Propeller.DiameterMm.Value / 25.4;
            double maxProp = c.Frame.MaxPropInch.Value;
            const double toleranceInch = 0.05;

            if (propInch <= maxProp + toleranceInch)
            { r.Status = "ok"; r.Message = $"Propelleri der -- {propInch:F1}\" <= {maxProp:F1}\" maks."; }
            else if (propInch <= maxProp + 0.35)
            { r.Status = "warning"; r.Message = $"Propelleri tuvu robežai -- {propInch:F1}\" pret {maxProp:F1}\" maks."; }
            else
            { r.Status = "critical"; r.Message = $"Propelleri par lielu -- {propInch:F1}\" > {maxProp:F1}\" maks."; }

            return r;
        }

        // #3 Rāmis -> "Stack" / Lidojuma kontroliera + Elektronisko ātruma regulatora stiprināšana
        private static CompatibilityResult Check03_StackMounting(BuildComponents c)
        {
            var r = new CompatibilityResult { CheckNumber = 3, Name = "Stiprinājumi (Lidojuma kontroliera + Elektronisko ātruma regulators)", Priority = 4 };

            if (c.Frame == null || (c.Fc == null && c.Esc == null))
            { r.Status = "unchecked"; r.Message = "Rāmis vai Lidojuma kontrolieris/Elektronisko ātruma regulators nav izvēlēti"; return r; }

            if (string.IsNullOrWhiteSpace(c.Frame.FcMountPattern))
            { r.Status = "unchecked"; r.Message = "Trūkst rāmja izmēra datu"; return r; }

            var issues = new List<string>();
            var warnings = new List<string>();
            string framePattern = c.Frame.FcMountPattern.ToLower();

            if (c.Fc != null)
            {
                if (c.Fc.MountPatternMm == null)
                    warnings.Add("Lidojuma kontroliera stiprinājuma raksts nav norādīts");
                else
                {
                    string fcSize = c.Fc.MountPatternMm.Value.ToString();
                    if (!framePattern.Contains(fcSize))
                        issues.Add($"Lidojuma kontroliera stiprinājums {c.Fc.MountPatternMm}mm neder rāmim ({c.Frame.FcMountPattern})");
                }
            }

            if (c.Esc != null)
            {
                if (c.Esc.MountPatternMm == null)
                    warnings.Add("Elektronisko ātruma regulatora stiprinājuma raksts nav norādīts");
                else
                {
                    string escSize = c.Esc.MountPatternMm.Value.ToString();
                    if (!framePattern.Contains(escSize))
                        issues.Add($"Elektronisko ātruma regulatora stiprinājums {c.Esc.MountPatternMm}mm neder rāmim ({c.Frame.FcMountPattern})");
                }
            }

            if (issues.Count > 0)
            { r.Status = "critical"; r.Message = string.Join("; ", issues); }
            else if (warnings.Count > 0)
            { r.Status = "warning"; r.Message = string.Join("; ", warnings); }
            else
            { r.Status = "ok"; r.Message = "Lidojuma kontrolieris/Elektronisko ātruma regulators der rāmim"; }

            return r;
        }

        // #4 Akumulatora sprieguma saderība
        private static CompatibilityResult Check04_BatteryVoltageFamily(BuildComponents c)
        {
            var r = new CompatibilityResult { CheckNumber = 4, Name = "Akumulatora spriegums", Priority = 5 };

            if (c.Battery == null)
            { r.Status = "unchecked"; r.Message = "Akumulators nav izvēlēts"; return r; }

            if (!c.Battery.CellCountS.HasValue)
            { r.Status = "unchecked"; r.Message = "Trūkst Akumulatoru šūnu skaita"; return r; }

            int batteryS = c.Battery.CellCountS.Value;
            var issues = new List<string>();
            var warnings = new List<string>();

            if (c.Motor != null)
            {
                if (c.Motor.RecommendedVoltageS == null)
                    warnings.Add("Motora ieteicamais spriegums (S) nav norādīts");
                else if (c.Motor.RecommendedVoltageS != batteryS)
                    issues.Add($"Motors iesaka {c.Motor.RecommendedVoltageS}S, bet Akumulatoram ir {batteryS}S");
            }

            if (c.Esc != null)
            {
                if (c.Esc.VoltageInputS == null)
                    warnings.Add("Elektronisko ātruma regulatora ieejas spriegums (S) nav norādīts");
                else if (batteryS > c.Esc.VoltageInputS)
                    issues.Add($"Elektronisko ātruma regulators atbalsta līdz {c.Esc.VoltageInputS}S, bet Akumulatoram ir {batteryS}S -- Elektronisko ātruma regulators sadegs!");
            }

            if (issues.Any(i => i.Contains("sadegs")))
            { r.Status = "critical"; r.Message = string.Join("; ", issues); }
            else if (issues.Count > 0)
            { r.Status = "warning"; r.Message = string.Join("; ", issues); }
            else if (warnings.Count > 0)
            { r.Status = "warning"; r.Message = string.Join("; ", warnings); }
            else if (c.Motor != null || c.Esc != null)
            { r.Status = "ok"; r.Message = $"Akumulators {batteryS}S ir saderīga"; }
            else
            { r.Status = "unchecked"; r.Message = "Nav motoru/Elektronisko ātruma regulators, lai pārbaudītu spriegumu"; }

            return r;
        }

        // #5 ESC strāvas stiprums pret motora prasībām
        private static CompatibilityResult Check05_EscCurrentRating(BuildComponents c)
        {
            var r = new CompatibilityResult { CheckNumber = 5, Name = "Elektronisko ātruma regulatora strāvas jauda", Priority = 5 };

            if (c.Esc == null || c.Motor == null)
            { r.Status = "unchecked"; r.Message = "Elektronisko ātruma regulators vai motors nav izvēlēti"; return r; }

            if (!c.Esc.ContinuousCurrentA.HasValue || !c.Motor.MaxThrustG.HasValue)
            { r.Status = "unchecked"; r.Message = "Trūkst strāvas vai vilces datu"; return r; }

            int cellCount = c.Battery?.CellCountS ?? c.Motor.RecommendedVoltageS ?? 4;
            double estPeakCurrent = EstimateMotorPeakCurrent(c.Motor.MaxThrustG.Value, cellCount);
            double escCurrent = c.Esc.ContinuousCurrentA.Value;

            if (escCurrent >= estPeakCurrent * 1.3)
            { r.Status = "ok"; r.Message = $"Elektronisko ātruma regulatora {escCurrent}A ir pietiekams (motora maks. ~{estPeakCurrent:F0}A)"; }
            else if (escCurrent >= estPeakCurrent)
            { r.Status = "warning"; r.Message = $"Elektronisko ātruma regulatora {escCurrent}A ir tuvu limitam (motora maks. ~{estPeakCurrent:F0}A)"; }
            else
            { r.Status = "critical"; r.Message = $"Elektronisko ātruma regulatora {escCurrent}A par vāju -- motors var patērēt ~{estPeakCurrent:F0}A maks.!"; }

            return r;
        }

        // #6 Vilces un svara attiecība (AUW)
        private static CompatibilityResult Check06_ThrustToWeight(BuildComponents c)
        {
            var r = new CompatibilityResult { CheckNumber = 6, Name = "Vilces un svara attiecība", Priority = 5 };

            if (c.Motor == null)
            { r.Status = "unchecked"; r.Message = "Motori nav izvēlēti"; return r; }

            if (!c.Motor.MaxThrustG.HasValue)
            { r.Status = "unchecked"; r.Message = "Trūkst motora Vilces datu"; return r; }

            int motorCount = GetMotorCount(c.Frame);
            double totalThrust = c.Motor.MaxThrustG.Value * motorCount;
            double totalWeight = CalculateTotalWeight(c);

            if (totalWeight <= 0)
            { r.Status = "unchecked"; r.Message = "Trūkst svara datu"; return r; }

            double ratio = totalThrust / totalWeight;

            if (ratio >= 4.0)
            { r.Status = "ok"; r.Message = $"Izcila vilce! Attiecība {ratio:F1}:1 (vilce {totalThrust:F0}g, svars {totalWeight:F0}g)"; }
            else if (ratio >= 3.0)
            { r.Status = "ok"; r.Message = $"Laba vilce. Attiecība {ratio:F1}:1 (vilce {totalThrust:F0}g, svars {totalWeight:F0}g)"; }
            else if (ratio >= 2.0)
            { r.Status = "warning"; r.Message = $"Tikko lidojams. Attiecība {ratio:F1}:1 (vilce {totalThrust:F0}g, svars {totalWeight:F0}g)"; }
            else
            { r.Status = "critical"; r.Message = $"Nelidos! Attiecība {ratio:F1}:1 (vilce {totalThrust:F0}g, svars {totalWeight:F0}g)"; }

            return r;
        }

        // #7 Propelleru <-> Motoru ieteikumi
        private static CompatibilityResult Check07_PropMotorMatch(BuildComponents c)
        {
            var r = new CompatibilityResult { CheckNumber = 7, Name = "Propelleru un motoru atbilstība", Priority = 4 };

            if (c.Propeller == null || c.Motor == null)
            { r.Status = "unchecked"; r.Message = "Propelleri vai motori nav izvēlēti"; return r; }

            var issues = new List<string>();
            var informational = new List<string>();

            if (!string.IsNullOrWhiteSpace(c.Propeller.RecommendedMotorSize))
            {
                if (!c.Motor.StatorSizeMm.HasValue)
                {
                    informational.Add("Motora izmērs nav norādīts");
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
                                issues.Add($"Propelleri iesaka motoru {c.Propeller.RecommendedMotorSize}, bet izvēlēts {c.Motor.StatorSizeMm}");
                        }
                        else
                        {
                            informational.Add("Motora izmēra ieteikuma formātu nevarēja pilnībā salīdzināt");
                        }
                    }
                }
            }

            if (c.Propeller.RecommendedMotorKv.HasValue)
            {
                if (!c.Motor.Kv.HasValue)
                {
                    informational.Add("Motora KV nav norādīts");
                }
                else
                {
                    double kvDiff = Math.Abs(c.Propeller.RecommendedMotorKv.Value - c.Motor.Kv.Value);
                    double kvPercent = kvDiff / c.Propeller.RecommendedMotorKv.Value;
                    if (kvPercent > 0.35)
                        issues.Add($"KV neatbilstība - propelleri iesaka ~{c.Propeller.RecommendedMotorKv}KV, motors ir {c.Motor.Kv}KV");
                }
            }

            if (c.Motor.RecommendedPropInch.HasValue && c.Propeller.DiameterMm.HasValue)
            {
                double propInch = c.Propeller.DiameterMm.Value / 25.4;
                double diff = Math.Abs(c.Motor.RecommendedPropInch.Value - propInch);
                if (diff > 0.5)
                    issues.Add($"Motors iesaka ~{c.Motor.RecommendedPropInch:F1}\" propellerus, izvēlēti {propInch:F1}\"");
            }

            if (issues.Count > 0)
            { r.Status = "warning"; r.Message = string.Join("; ", issues); }
            else
            {
                r.Status = "ok";
                r.Message = informational.Count > 0
                    ? $"Propelleri un motori ir saderīgi ({string.Join("; ", informational)})"
                    : "Propelleri atbilst motoriem";
            }

            return r;
        }

        // #8 Akumulatora C-reitings / pīķis pret maksimālo strāvas patēriņu
        private static CompatibilityResult Check08_BatteryCRating(BuildComponents c)
        {
            var r = new CompatibilityResult { CheckNumber = 8, Name = "Akumulatora C-reitings", Priority = 4 };

            if (c.Battery == null || c.Motor == null)
            { r.Status = "unchecked"; r.Message = "Akumulators vai motori nav izvēlēti"; return r; }

            if (!c.Battery.BurstRateC.HasValue || !c.Battery.CapacityMah.HasValue || !c.Motor.MaxThrustG.HasValue)
            { r.Status = "unchecked"; r.Message = "Trūkst C-reitinga vai kapacitātes datu"; return r; }

            int cellCount = c.Battery.CellCountS ?? 4;
            double peakPerMotor = EstimateMotorPeakCurrent(c.Motor.MaxThrustG.Value, cellCount);
            int motorCount = GetMotorCount(c.Frame);
            double totalPeakCurrent = peakPerMotor * motorCount;
            double batteryMaxBurst = c.Battery.BurstRateC.Value * (c.Battery.CapacityMah.Value / 1000.0);

            if (batteryMaxBurst >= totalPeakCurrent * 1.2)
            { r.Status = "ok"; r.Message = $"Akumulators spēs tikt galā -- {batteryMaxBurst:F0}A maks. >= {totalPeakCurrent:F0}A nepieciešams"; }
            else if (batteryMaxBurst >= totalPeakCurrent)
            { r.Status = "warning"; r.Message = $"Akumulators tuvu limitam -- {batteryMaxBurst:F0}A maks. pret {totalPeakCurrent:F0}A nepieciešams"; }
            else
            { r.Status = "critical"; r.Message = $"Akumulators par vāju! {batteryMaxBurst:F0}A maks. < {totalPeakCurrent:F0}A nepieciešams"; }

            return r;
        }

        // #9 Lidojuma kontroliera programmaparatūra atbalsta radio protokolu
        private static CompatibilityResult Check09_FcFirmwareProtocol(BuildComponents c)
        {
            var r = new CompatibilityResult { CheckNumber = 9, Name = "Lidojuma kontroliera programmaparatūra / Radio protokols", Priority = 4 };

            if (c.Fc == null || c.Receiver == null)
            { r.Status = "unchecked"; r.Message = "Lidojuma kontrolieris vai uztvērējs nav izvēlēts"; return r; }

            if (string.IsNullOrWhiteSpace(c.Fc.FirmwareSupport) || string.IsNullOrWhiteSpace(c.Receiver.Protocol))
            { r.Status = "unchecked"; r.Message = "Trūkst programmaparatūras/protokola datu"; return r; }

            string firmware = c.Fc.FirmwareSupport.ToLower();
            string protocol = c.Receiver.Protocol.ToLower();

            if (firmware.Contains("betaflight") || firmware.Contains("inav"))
            { r.Status = "ok"; r.Message = $"Lidojuma kontroliera programmaparatūra ({c.Fc.FirmwareSupport}) atbalsta {c.Receiver.Protocol} caur UART"; }
            else
            { r.Status = "ok"; r.Message = $"Lidojuma kontroliera programmaparatūra: {c.Fc.FirmwareSupport}, Protokols: {c.Receiver.Protocol} -- pārbaudiet manuāli"; }

            return r;
        }

        // #10 Uztvērēja <-> Radio pults protokols
        private static CompatibilityResult Check10_ReceiverRadioProtocol(BuildComponents c)
        {
            var r = new CompatibilityResult { CheckNumber = 10, Name = "Uztvērēja / Pults protokols", Priority = 4 };

            if (c.Receiver == null || c.RadioController == null)
            { r.Status = "unchecked"; r.Message = "Uztvērējs vai pults nav izvēlēta"; return r; }

            if (string.IsNullOrWhiteSpace(c.Receiver.Protocol) || string.IsNullOrWhiteSpace(c.RadioController.ProtocolsSupported))
            { r.Status = "unchecked"; r.Message = "Trūkst protokola datu"; return r; }

            string rxProtocol = c.Receiver.Protocol.Trim().ToLower();
            string txProtocols = c.RadioController.ProtocolsSupported.ToLower();

            if (txProtocols.Contains(rxProtocol))
            { r.Status = "ok"; r.Message = $"Protokoli sakrīt -- {c.Receiver.Protocol}"; }
            else
            { r.Status = "critical"; r.Message = $"Protokolu neatbilstība! Uztvērējs: {c.Receiver.Protocol}, Pults: {c.RadioController.ProtocolsSupported}"; }

            return r;
        }

        // #11 Uztvērēja antenas saderība
        private static CompatibilityResult Check11_ReceiverAntenna(BuildComponents c)
        {
            var r = new CompatibilityResult { CheckNumber = 11, Name = "Uztvērēja antena", Priority = 3 };

            if (c.Receiver == null || c.ReceiverAntenna == null)
            { r.Status = "unchecked"; r.Message = "Uztvērējs vai antena nav izvēlēta"; return r; }

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
                    r.Message = "Antena ir saderīga ar uztvērēju";
                }
                else if (bandMatches)
                {
                    r.Status = "ok";
                    r.Message = $"Frekvenču josla sakrīt ({c.Receiver.FrequencyBand}) - tieša modeļa atbilstība nav rasta";
                }
                else if (bandKnown)
                {
                    r.Status = "warning";
                    r.Message = $"Uztvērējs nav antenas saderības sarakstā un frekvences atšķiras ({c.ReceiverAntenna.FrequencyBand} pret {c.Receiver.FrequencyBand}) - pārbaudiet manuāli";
                }
                else
                {
                    r.Status = "warning";
                    r.Message = "Uztvērējs nav antenas saderības sarakstā - pārbaudiet manuāli";
                }
            }
            else
            {
                if (bandMatches)
                { r.Status = "ok"; r.Message = $"Frekvenču josla sakrīt ({c.Receiver.FrequencyBand})"; }
                else if (bandKnown)
                { r.Status = "warning"; r.Message = $"Frekvenču josla var nesakrist ({c.ReceiverAntenna.FrequencyBand} pret {c.Receiver.FrequencyBand})"; }
                else
                { r.Status = "unchecked"; r.Message = "Trūkst saderības datu"; }
            }

            return r;
        }

        // #12 Video sistēma -- analogā pret digitālo
        private static CompatibilityResult Check12_VideoSystem(BuildComponents c)
        {
            var r = new CompatibilityResult { CheckNumber = 12, Name = "Video sistēma (Analogā/Digitālā)", Priority = 4 };

            if (c.Camera == null && c.Vtx == null && c.FpvGoggles == null)
            { r.Status = "unchecked"; r.Message = "Video komponentes nav izvēlētas"; return r; }

            var systems = new List<string>();
            if (!string.IsNullOrWhiteSpace(c.Camera?.TypeSystem)) systems.Add(c.Camera.TypeSystem.Trim());
            if (!string.IsNullOrWhiteSpace(c.Vtx?.Type)) systems.Add(c.Vtx.Type.Trim());
            if (!string.IsNullOrWhiteSpace(c.FpvGoggles?.VideoSystem)) systems.Add(c.FpvGoggles.VideoSystem.Trim());

            if (systems.Count < 2)
            { r.Status = "unchecked"; r.Message = "Nav pietiekami daudz video komponentu saderības pārbaudei"; return r; }

            var normalized = systems.Select(NormalizeVideoSystem).ToList();

            if (normalized.Distinct().Count() == 1)
            { r.Status = "ok"; r.Message = $"Video sistēma ir vienota - {systems[0]}"; }
            else
            { r.Status = "critical"; r.Message = $"Video sistēmu neatbilstība! Brillēs nebūs attēla. ({string.Join(", ", systems)})"; }

            return r;
        }

        // #13 Video raidītāja antenas savienotājs
        private static CompatibilityResult Check13_VtxAntennaConnector(BuildComponents c)
        {
            var r = new CompatibilityResult { CheckNumber = 13, Name = "Video raidītāja antenas savienotājs", Priority = 4 };

            if (c.Vtx == null || c.VideoAntenna == null)
            { r.Status = "unchecked"; r.Message = "Video raidītājs vai video raidītāja antena nav izvēlēta"; return r; }

            if (string.IsNullOrWhiteSpace(c.Vtx.AntennaConnector) || string.IsNullOrWhiteSpace(c.VideoAntenna.Connector))
            { r.Status = "unchecked"; r.Message = "Trūkst informācijas par savienotāju"; return r; }

            if (c.Vtx.AntennaConnector.Equals(c.VideoAntenna.Connector, StringComparison.OrdinalIgnoreCase))
            { r.Status = "ok"; r.Message = $"Savienotājs sakrīt ({c.Vtx.AntennaConnector})"; }
            else
            { r.Status = "warning"; r.Message = $"Savienotāju neatbilstība! Video raidītājs: {c.Vtx.AntennaConnector}, Antena: {c.VideoAntenna.Connector} -- izmantojiet adapteri vai Video raidītāja komplektā esošo antenu"; }

            return r;
        }

        // #14 Video raidītāja jauda / siltuma izkliede
        private static CompatibilityResult Check14_VtxPowerHeat(BuildComponents c)
        {
            var r = new CompatibilityResult { CheckNumber = 14, Name = "Video raidītāja jauda", Priority = 3 };

            if (c.Vtx == null)
            { r.Status = "unchecked"; r.Message = "Video raidītājs nav izvēlēts"; return r; }

            if (!c.Vtx.MaxPowerMw.HasValue)
            { r.Status = "unchecked"; r.Message = "Trūkst Video raidītāja jaudas datu"; return r; }

            if (c.Vtx.MaxPowerMw <= 600)
            { r.Status = "ok"; r.Message = $"Video raidītāja jauda {c.Vtx.MaxPowerMw}mW - pieņemama labi vēdinātiem droniem"; }
            else if (c.Vtx.MaxPowerMw <= 1200)
            { r.Status = "warning"; r.Message = $"Video raidītāja jauda {c.Vtx.MaxPowerMw}mW - ieteicama laba ventilācija"; }
            else
            { r.Status = "warning"; r.Message = $"Video raidītāja jauda {c.Vtx.MaxPowerMw}mW - liels karstums, nepieciešama aktīva dzesēšana"; }

            return r;
        }

        // #15 Lidojuma kontroliera ieejas sprieguma saderība
        private static CompatibilityResult Check15_FcVoltageInput(BuildComponents c)
        {
            var r = new CompatibilityResult { CheckNumber = 15, Name = "Lidojuma kontroliera ieejas spriegums", Priority = 3 };

            if (c.Fc == null || c.Battery == null)
            { r.Status = "unchecked"; r.Message = "Lidojuma kontrolieris vai Akumulators nav izvēlēta"; return r; }

            if (string.IsNullOrWhiteSpace(c.Fc.VoltageInputS) || !c.Battery.CellCountS.HasValue)
            { r.Status = "unchecked"; r.Message = "Trūkst sprieguma datu"; return r; }

            var (minS, maxS) = ParseVoltageRange(c.Fc.VoltageInputS);
            int batteryS = c.Battery.CellCountS.Value;

            if (minS == 0 && maxS == 0)
            { r.Status = "unchecked"; r.Message = $"Nevar nolasīt Lidojuma kontroliera sprieguma diapazonu ({c.Fc.VoltageInputS})"; return r; }

            if (batteryS >= minS && batteryS <= maxS)
            { r.Status = "ok"; r.Message = $"Lidojuma kontrolieris atbalsta {batteryS}S (diapazons: {minS}-{maxS}S)"; }
            else
            { r.Status = "critical"; r.Message = $"Lidojuma kontrolieris neatbalsta {batteryS}S! Diapazons: {minS}-{maxS}S"; }

            return r;
        }

        // #16 Kopējais svars -- zem 250g vai limiti
        private static CompatibilityResult Check16_TotalWeight(BuildComponents c)
        {
            var r = new CompatibilityResult { CheckNumber = 16, Name = "Kopējais svars", Priority = 2 };

            var missingWeights = GetMissingWeightWarnings(c);
            double totalWeight = CalculateTotalWeight(c);

            if (totalWeight <= 0 && missingWeights.Count == 0)
            { r.Status = "unchecked"; r.Message = "Trūkst svara datu"; return r; }

            if (missingWeights.Count > 0)
            {
                string missingList = string.Join("; ", missingWeights);
                if (totalWeight > 0 && totalWeight <= 250 && missingWeights.Count <= 1)
                {
                    r.Status = "ok";
                    r.Message = $"Aptuveni zem 250g ({totalWeight:F0}g) ar nelieliem trūkstošiem datiem ({missingList})";
                }
                else if (totalWeight > 0)
                {
                    r.Status = "warning";
                    r.Message = $"Daļējs svars {totalWeight:F0}g -- faktiskais kopsvars būs lielāks ({missingList})";
                }
                else
                {
                    r.Status = "warning";
                    r.Message = $"Svars nav norādīts: {missingList}";
                }
                return r;
            }

            if (totalWeight <= 250)
            { r.Status = "ok"; r.Message = $"Zem 250g ({totalWeight:F0}g) - valsts reģistrācija nav nepieciešama"; }
            else if (totalWeight <= 500)
            { r.Status = "warning"; r.Message = $"Kopējais svars {totalWeight:F0}g - pārsniedz 250g limitu, nepieciešama reģistrācija"; }
            else
            { r.Status = "warning"; r.Message = $"Kopējais svars {totalWeight:F0}g"; }

            return r;
        }

        // #17 Aplēstais lidojuma laiks (bonuss)
        private static CompatibilityResult Check17_EstFlightTime(BuildComponents c)
        {
            var r = new CompatibilityResult { CheckNumber = 17, Name = "Aptuvenais lidojuma laiks", Priority = 2 };

            if (c.Battery == null || c.Motor == null)
            { r.Status = "unchecked"; r.Message = "Akumulators vai motori nav izvēlēti"; return r; }

            if (!c.Battery.CapacityMah.HasValue || !c.Battery.CellCountS.HasValue || !c.Motor.MaxThrustG.HasValue)
            { r.Status = "unchecked"; r.Message = "Trūkst kapacitātes/vilkmes datu"; return r; }

            double totalWeight = CalculateTotalWeight(c);
            if (totalWeight <= 0)
            { r.Status = "unchecked"; r.Message = "Trūkst svara datu lidojuma laika aprēķinam"; return r; }

            int cellCount = c.Battery.CellCountS.Value;
            double voltage = cellCount * 3.7;
            int motorCount = GetMotorCount(c.Frame);

            // Karāšanās (Hover): katrs motors rada vilkmi = kopsvars / motoru skaits
            double hoverThrustPerMotor = totalWeight / motorCount;
            double hoverPowerPerMotor = hoverThrustPerMotor / 5.0; // ~5 g/W karāšanās režīmā
            double hoverCurrentPerMotor = hoverPowerPerMotor / voltage;
            double totalHoverCurrent = hoverCurrentPerMotor * motorCount;

            double usableCapacityAh = c.Battery.CapacityMah.Value / 1000.0 * 0.8; // 80% izmantojams
            double flightTimeMinutes = (usableCapacityAh / totalHoverCurrent) * 60;

            r.Status = "ok";
            r.Message = $"~{flightTimeMinutes:F1} min (aplēstais karāšanās laiks ar 80% Akumulatoru)";
            return r;
        }

        // --- Palīgmetodes (ziņojumu daļas) ---

       

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
            if (c.Frame != null && c.Frame.WeightG == null) missing.Add("Rāmja svars nav norādīts");
            if (c.Motor != null && c.Motor.WeightG == null) missing.Add("Motora svars nav norādīts");
            if (c.Propeller != null && c.Propeller.WeightG == null) missing.Add("Propellera svars nav norādīts");
            if (c.Esc != null && c.Esc.WeightG == null) missing.Add("Elektronisko ātruma regulatora svars nav norādīts");
            if (c.Battery != null && c.Battery.WeightG == null) missing.Add("Akumulatora svars nav norādīts");
            if (c.Fc != null && c.Fc.WeightG == null) missing.Add("Lidojuma kontroliera svars nav norādīts");
            if (c.Camera != null && c.Camera.WeightG == null) missing.Add("Kameras svars nav norādīts");
            if (c.Vtx != null && c.Vtx.WeightG == null) missing.Add("Video raidītāja svars nav norādīts");
            if (c.VideoAntenna != null && c.VideoAntenna.WeightG == null) missing.Add("Video raidītāja antenas svars nav norādīts");
            if (c.Receiver != null && c.Receiver.WeightG == null) missing.Add("Radio uztvērēja svars nav norādīts");
            if (c.ReceiverAntenna != null && c.ReceiverAntenna.WeightG == null) missing.Add("Radio uztvērēja antenas svars nav norādīts");
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
