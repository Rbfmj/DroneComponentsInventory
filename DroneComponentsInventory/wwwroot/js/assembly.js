document.addEventListener('DOMContentLoaded', function () {
    //Dati par pašreizējo uzbūvi
    var build = window.DRONE_BUILD;
    //Drona id  
    var buildId = build && build.buildId ? Number(build.buildId) : 0;
    //HTML pogas un elementi
    const modeToggleButton = document.getElementById('modeToggleButton');
    const snapToggleButton = document.getElementById('snapToggleButton');
    const saveLayoutButton = document.getElementById('saveLayoutButton');
    const stepListElement = document.getElementById('step-list');
    const stepCounterElement = document.getElementById('step-counter');
    const prevStepButton = document.getElementById('btn-prev');
    const nextStepButton = document.getElementById('btn-next');
    //Automātiskā pāreja uz nākamo soli
    let guideAutoAdvanceHandle = null;
    //snap funkcija ir ieslēgta
    let isSnapEnabled = true;
    //Pašreizējais režīms: 'guide' vai 'developer'
    let currentMode = 'guide';
    //mācību solis kurā sākt
    let currentGuideStepIndex = 0;
    //Pabeigtie mācību soļi
    let guideCompletedSteps = new Set();
    //Sāktās komponentes mācību režīmā
    let guideStartedParts = new Set();
    //Saglabāts stāvoklis pārslēgšanās brīdī
    let developerCanvasSnapshot = null;
    let developerSnapEnabledBeforeGuide = true;
    let hasSavedLayout = false;

    const container = document.getElementById('container');
    if (!container) {
        console.error('Container #container not found');
        return;
    }

    const workbenchArea = container.parentElement;


    //Aprēķina kanvas izmēru
    function getContentSize(el) {
        const s = window.getComputedStyle(el);
        return {
            width:  el.clientWidth  - parseFloat(s.paddingLeft)  - parseFloat(s.paddingRight),
            height: el.clientHeight - parseFloat(s.paddingTop)   - parseFloat(s.paddingBottom)
        };
    }

    let { width, height } = getContentSize(workbenchArea);

    console.log(`Canvas size: ${width}x${height}`);

    //Izveido Konva Stage

    const stage = new Konva.Stage({
        container: 'container',
        width: width,
        height: height
    });

    //Izveido Konva Layer
    const layer = new Konva.Layer();
    stage.add(layer);

    //Visi drona komponenti ar sākuma pozīcijām
    const PARTS = {
        'frame': {
            image: '/images/parts/frame.png',
            x: 0.5, y: 0.5, w: 0.8, h: 0.8,
            draggable: true,
            name: 'BASE FRAME',
            id: 'frame'
        },
        'rear-plate': {
            image: '/images/parts/rear-plate.png',
            x: 0.5, y: 0.65, w: 0.4, h: 0.25,
            draggable: true,
            name: 'REAR PLATE',
            id: 'rear-plate'
        },
        'front-plate': {
            image: '/images/parts/front-plate.png',
            x: 0.5, y: 0.35, w: 0.4, h: 0.25,
            draggable: true,
            name: 'FRONT PLATE',
            id: 'front-plate'
        },
        'motor-fl': {
            image: '/images/parts/motor.png',
            x: 0.25, y: 0.25, w: 0.1, h: 0.1,
            draggable: true,
            name: 'MOTOR 1',
            id: 'motor-fl'
        },
        'motor-fr': {
            image: '/images/parts/motor.png',
            x: 0.75, y: 0.25, w: 0.1, h: 0.1,
            draggable: true,
            name: 'MOTOR 2',
            id: 'motor-fr'
        },
        'motor-bl': {
            image: '/images/parts/motor.png',
            x: 0.25, y: 0.75, w: 0.1, h: 0.1,
            draggable: true,
            name: 'MOTOR 3',
            id: 'motor-bl'
        },
        'motor-br': {
            image: '/images/parts/motor.png',
            x: 0.75, y: 0.75, w: 0.1, h: 0.1,
            draggable: true,
            name: 'MOTOR 4',
            id: 'motor-br'
        },
        'fc': {
            image: '/images/parts/fc-esc.png',
            x: 0.5, y: 0.5, w: 0.12, h: 0.12,
            draggable: true,
            name: 'FLIGHT CONTROLLER',
            id: 'fc'
        },
        'capacitor': {
            image: '/images/parts/cap.png',
            x: 0.35, y: 0.5, w: 0.08, h: 0.08,
            draggable: true,
            name: 'CAPACITOR & BATTERY LEAD',
            id: 'capacitor'
        },
        'camera': {
            image: '/images/parts/fpvcamera.png',
            x: 0.5, y: 0.15, w: 0.09, h: 0.07,
            draggable: true,
            name: 'CAMERA',
            id: 'camera'
        },
        'vtx': {
            image: '/images/parts/vtx.png',
            x: 0.5, y: 0.85, w: 0.1, h: 0.1,
            draggable: true,
            name: 'VTX + ANTENNA',
            id: 'vtx'
        },
        'receiver': {
            image: '/images/parts/receiver+antenna.png',
            x: 0.5, y: 0.72, w: 0.1, h: 0.1,
            draggable: true,
            name: 'RECEIVER',
            id: 'receiver'
        },
        'propeller-fl': {
            image: '/images/parts/propeller.png',
            x: 0.075, y: 0.075, w: 0.15, h: 0.15,
            draggable: true,
            name: 'PROPELLER 1',
            id: 'propeller-fl'
        },
        'propeller-fr': {
            image: '/images/parts/propeller.png',
            x: 0.925, y: 0.075, w: 0.15, h: 0.15,
            draggable: true,
            name: 'PROPELLER 2',
            id: 'propeller-fr'
        },
        'propeller-bl': {
            image: '/images/parts/propeller.png',
            x: 0.075, y: 0.925, w: 0.15, h: 0.15,
            draggable: true,
            name: 'PROPELLER 3',
            id: 'propeller-bl'
        },
        'propeller-br': {
            image: '/images/parts/propeller.png',
            x: 0.925, y: 0.925, w: 0.15, h: 0.15,
            draggable: true,
            name: 'PROPELLER 4',
            id: 'propeller-br'
        },
        'top-frame': {
            image: '/images/parts/top-plate.png',
            x: 0.5, y: 0.5, w: 0.25, h: 0.25,
            draggable: true,
            name: 'TOP FRAME',
            id: 'top-frame'
        },
        'battery-top': {
            image: '/images/parts/battery.png',
            x: 0.5, y: 0.5, w: 0.15, h: 0.2,
            draggable: true,
            name: 'BATTERY (TOP)',
            id: 'battery-top'
        }
    };


    //Pozīcijas, kur komponentes jāliek

    const SNAP_POSITIONS = {
        'battery-top': { x: 0.5161, y: 0.5087 },
        'camera': { x: 0.5023, y: 0.1936 },
        'capacitor': { x: 0.5576, y: 0.5697 },
        'fc': { x: 0.5031, y: 0.5053 },
        'frame': { x: 0.5000, y: 0.5000 },
        'front-plate': { x: 0.5032, y: 0.4052 },
        'motor-bl': { x: 0.7754, y: 0.2738 },
        'motor-br': { x: 0.7842, y: 0.7312 },
        'motor-fl': { x: 0.2234, y: 0.7263 },
        'motor-fr': { x: 0.2125, y: 0.2672 },
        'propeller-bl': { x: 0.2136, y: 0.2662 },
        'propeller-br': { x: 0.2208, y: 0.7275 },
        'propeller-fl': { x: 0.7818, y: 0.7320 },
        'propeller-fr': { x: 0.7782, y: 0.2773 },
        'rear-plate': { x: 0.4953, y: 0.6566 },
        'receiver': { x: 0.5046, y: 0.7142 },
        'top-frame': { x: 0.5100, y: 0.5097 },
        'vtx': { x: 0.5049, y: 0.7153 }
    };


    //Rotācija un mērogs katrai komponentei
    const PART_TRANSFORMS = {
        'frame': { rotation: 0, scaleX: 1, scaleY: 1 },
        'rear-plate': { rotation: 0, scaleX: 1, scaleY: 1 },
        'front-plate': { rotation: 0, scaleX: 1, scaleY: 1 },
        'motor-fl': { rotation: 0, scaleX: 1, scaleY: 1 },
        'motor-fr': { rotation: 0, scaleX: 1, scaleY: 1 },
        'motor-bl': { rotation: 0, scaleX: 1, scaleY: 1 },
        'motor-br': { rotation: 0, scaleX: 1, scaleY: 1 },
        'fc': { rotation: 0, scaleX: 1, scaleY: 1 },
        'capacitor': { rotation: 0, scaleX: 1, scaleY: 1 },
        'camera': { rotation: 0, scaleX: 1, scaleY: 1 },
        'receiver': { rotation: 0, scaleX: 1, scaleY: 1 },
        'vtx': { rotation: 0, scaleX: 1, scaleY: 1 },
        'propeller-fl': { rotation: 0, scaleX: 1, scaleY: 1 },
        'propeller-fr': { rotation: 0, scaleX: 1, scaleY: 1 },
        'propeller-bl': { rotation: 0, scaleX: 1, scaleY: 1 },
        'propeller-br': { rotation: 0, scaleX: 1, scaleY: 1 },
        'top-frame': { rotation: 0, scaleX: 1, scaleY: 1 },
        'battery-top': { rotation: 0, scaleX: 1, scaleY: 1 }
    };


    //Attālums, ja netrāpa komponenti precīzi noteiktajā pozīcijā
    const SNAP_THRESHOLD_RATIO = 0.08;
    //Sākuma rindas apakšā
    const BOTTOM_START_ROWS = [0.72, 0.84, 0.94];
    const START_SIDE_PADDING_RATIO = 0.08;

    const DRAGGABLE_PART_IDS = Object.keys(PARTS).filter(partId => PARTS[partId].draggable);


    //Pašlaik izvēlētā komponente
    let selectedPartId = null;

    //Mācību soļi
    const GUIDE_STEPS = [
        {
            title: 'Add rear plate',
            description: 'Place the rear plate onto the frame.',
            partIds: ['rear-plate']
        },
        {
            title: 'Add front plate',
            description: 'Place the front plate onto the front section of the frame.',
            partIds: ['front-plate']
        },
        {
            title: 'Add a flight controller',
            description: 'Install the flight controller into the center stack area.',
            partIds: ['fc']
        },
        {
            title: 'Add capacitor & battery lead',
            description: 'Place the capacitor and battery lead before moving on.',
            partIds: ['capacitor']
        },
        {
            title: 'Add all 4 motors',
            description: 'Install all four motors onto the frame arms. Any motor can go to any arm.',
            partIds: ['motor-fl', 'motor-fr', 'motor-bl', 'motor-br']
        },
        {
            title: 'Add VTX and antenna',
            description: 'Place the VTX and antenna assembly onto the rear section.',
            partIds: ['vtx']
        },
        {
            title: 'Add camera',
            description: 'Install the FPV camera onto the front of the frame.',
            partIds: ['camera']
        },
        {
            title: 'Add receiver and antenna',
            description: 'Place the receiver and route its antenna as part of this step.',
            partIds: ['receiver']
        },
        {
            title: 'Smoke check',
            description: 'Perform a smoke check before continuing.',
            partIds: [],
            isAction: true,
            actionLabel: 'Mark Smoke Check Complete'
        },
        {
            title: 'Add a top frame',
            description: 'Install the top frame plate.',
            partIds: ['top-frame']
        },
        {
            title: 'Add all propellers',
            description: 'Install all four propellers onto the motors. Any propeller can go to any motor.',
            partIds: ['propeller-fl', 'propeller-fr', 'propeller-bl', 'propeller-br']
        },
        {
            title: 'Install battery on top frame plate',
            description: 'Place the battery onto the top frame plate.',
            partIds: ['battery-top']
        },
        {
            title: 'Bind receiver to radio controller',
            description: 'Bind the receiver to the radio controller before setup.',
            partIds: [],
            isAction: true,
            actionLabel: 'Mark Receiver Bound'
        },
        {
            title: 'Bind goggles and VTX',
            description: 'Bind the goggles to the VTX.',
            partIds: [],
            isAction: true,
            actionLabel: 'Mark Goggles Bound'
        },
        {
            title: 'Configure the drone',
            description: 'Configure motors, controller, goggles, and the rest of the setup.',
            partIds: [],
            isAction: true,
            actionLabel: 'Mark Configuration Complete'
        },
        {
            title: 'Congratulations',
            description: 'The drone build is complete.',
            partIds: [],
            isAction: true,
            actionLabel: 'Finish Build'
        }
    ];


    //Grupas, kuras var mainīt vietām
    const INTERCHANGEABLE_GROUPS = {
        'motors': ['motor-fl', 'motor-fr', 'motor-bl', 'motor-br'],
        'propellers': ['propeller-fl', 'propeller-fr', 'propeller-bl', 'propeller-br']
    };

    function getInterchangeableGroup(partId) {
        for (const members of Object.values(INTERCHANGEABLE_GROUPS)) {
            if (members.includes(partId)) return members;
        }
        return null;
    }

    //Seko līdzi visām koordinātēm, rotācijai un mēroga
    const positionTracker = {
        //glabā pašreizējās pozīcijas katrai daļai
        positions: {},
        //saglabā vēsturi visām pozīciju izmaiņām
        history: [],
        //maksimālais ierakstu skaits vēsturē
        maxHistory: 50,
        //Atgriež vienas daļas pašreizējo pozīciju
        getPosition: function(partId) {
            const konvaImage = layer.findOne(`#${partId}`);
            if (!konvaImage) return null;

            return {
                partId: partId,
                partName: PARTS[partId]?.name || 'Unknown',
                pixelX: konvaImage.x(),
                pixelY: konvaImage.y(),
                percentX: (konvaImage.x() / width).toFixed(4),
                percentY: (konvaImage.y() / height).toFixed(4),
                rotation: Number(konvaImage.rotation().toFixed(2)),
                scaleX: Number(konvaImage.scaleX().toFixed(4)),
                scaleY: Number(konvaImage.scaleY().toFixed(4)),
                pixelWidth: Number((konvaImage.width() * Math.abs(konvaImage.scaleX())).toFixed(2)),
                pixelHeight: Number((konvaImage.height() * Math.abs(konvaImage.scaleY())).toFixed(2)),
                timestamp: new Date().toLocaleTimeString(),
                status: 'current'
            };
        },

        //Atgriež pozīcijas visām daļām
        getAllPositions: function() {
            const allPos = {};
            Object.keys(PARTS).forEach(partId => {
                const pos = this.getPosition(partId);
                if (pos) allPos[partId] = pos;
            });
            return allPos;
        },

        //Saglabā daļas pozīciju vēsturē un kā pašreizējo
        trackPosition: function(partId) {
            const pos = this.getPosition(partId);
            if (!pos) return;

            //Pievieno vēsturei
            this.history.push(pos);
            if (this.history.length > this.maxHistory) {
                //izdzēš vecāko ierakstu
                this.history.shift();
            }

            //Atjaunina pašreizējo pozīciju
            this.positions[partId] = pos;

            return pos;
        },

        //Eksportē pozīcijas kā JavaScript kodu
        exportAsCode: function() {
            let code = 'const SNAP_POSITIONS = {\n';
            Object.entries(this.positions).forEach(([partId, pos]) => {
                code += `    '${partId}': { x: ${pos.percentX}, y: ${pos.percentY} },\n`;
            });
            code += '};\n\nconst PART_TRANSFORMS = {\n';
            Object.entries(this.positions).forEach(([partId, pos]) => {
                code += `    '${partId}': { rotation: ${pos.rotation}, scaleX: ${pos.scaleX}, scaleY: ${pos.scaleY} },\n`;
            });
            code += '};';
            return code;
        },

        //Eksportē pozīcijas kā JSON
        exportAsJSON: function() {
            return JSON.stringify(this.positions, null, 2);
        },

        //Atgriež vēsturi konkrētai daļai
        getHistory: function(partId) {
            return this.history.filter(h => h.partId === partId);
        },

        //Atgriež pēdējo saglabāto pozīciju konkrētai daļai
        getLastPosition: function(partId) {
            const history = this.getHistory(partId);
            return history[history.length - 1] || null;
        },

        //Izdrukā konsolē visas pašreizējās pozīcijas
        logAllPositions: function() {
            console.clear();
            console.log('=== CURRENT POSITIONS ===\n');
            Object.entries(this.positions).forEach(([partId, pos]) => {
                console.log(`${pos.partName}:`);
                console.log(`  Pixels: (${pos.pixelX.toFixed(0)}, ${pos.pixelY.toFixed(0)})`);
                console.log(`  Percent: (${pos.percentX}, ${pos.percentY})`);
                console.log(`  Rotation: ${pos.rotation}°`);
                console.log(`  Scale: (${pos.scaleX}, ${pos.scaleY})`);
                console.log(`  Size: ${pos.pixelWidth}px × ${pos.pixelHeight}px`);
                console.log('');
            });
        },

        //Izdrukā konsolē gatavo kodu
        logAsCode: function() {
            console.clear();
            console.log(this.exportAsCode());
        },

        //Saglabā pozīcijas localStorage
        saveToLocalStorage: function() {
            localStorage.setItem('dronePositions', this.exportAsJSON());
            console.log('✓ Positions saved to localStorage');
        },

        //Ielādē pozīcijas no localStorage
        loadFromLocalStorage: function() {
            const saved = localStorage.getItem('dronePositions');
            if (!saved) {
                console.log('✗ No saved positions found');
                return;
            }
            this.positions = JSON.parse(saved);
            console.log('✓ Positions loaded from localStorage');
        }
    };

    //Atgriež pašreizējo snap izkārtojumu
    function getCurrentSnapLayout() {
        //procentuālās X un Y koordinātes
        const snapPositions = {};
        //rotācija un mērogs katrai daļai
        const partTransforms = {};
        //Iet cauri visām definētajām komponentu objektā
        Object.keys(PARTS).forEach(partId => {
            const pos = positionTracker.getPosition(partId);
            if (!pos) {
                //ja komponente nav atrasta, izlaiž
                return;
            }
            //Saglabā pozīciju procentos
            snapPositions[partId] = {
                x: Number(pos.percentX),
                y: Number(pos.percentY)
            };
            //Saglabā rotāciju un mērogu
            partTransforms[partId] = {
                rotation: pos.rotation,
                scaleX: pos.scaleX,
                scaleY: pos.scaleY
            };
        });

        return {
            snapPositions: snapPositions,
            partTransforms: partTransforms,
            //Saglabāšanas laiks
            savedAt: new Date().toISOString()
        };
    }

    //Uzliek saglabāto snap izkārtojumu
    function applySnapLayout(layout) {
        if (!layout) {
            return;
        }
        //Atjaunina pozīcijas
        Object.entries(layout.snapPositions || {}).forEach(([partId, value]) => {
            if (!SNAP_POSITIONS[partId]) {
                SNAP_POSITIONS[partId] = {};
            }
            //Saglabā X un Y koordinātes tikai ja tas ir skaitlis
            SNAP_POSITIONS[partId].x = typeof value.x === 'number' ? value.x : SNAP_POSITIONS[partId].x;
            SNAP_POSITIONS[partId].y = typeof value.y === 'number' ? value.y : SNAP_POSITIONS[partId].y;
        });

        //Atjaunina rotāciju un mērogu
        Object.entries(layout.partTransforms || {}).forEach(([partId, value]) => {
            //Katrai vērtībai pārbauda, vai tā ir derīgs skaitlis
            PART_TRANSFORMS[partId] = {
                rotation: typeof value.rotation === 'number' ? value.rotation : 0,
                scaleX: typeof value.scaleX === 'number' ? value.scaleX : 1,
                scaleY: typeof value.scaleY === 'number' ? value.scaleY : 1
            };
        });
    }


    //Atjaunina visu Snap komponenta pozīcijas, rotāciju un mērogu
    function applySnapLayoutToCanvas() {
        //Iet cauri visām komponentēm un uzliek saglabātās koordinātes + transformācijas
        Object.keys(PARTS).forEach(partId => {
            //atrod Konva mezglu pēc id
            const node = getPartNode(partId);
            if (!node) {
                return;
            }
            //Uzliek pozīciju pikseļos
            const snapPosition = SNAP_POSITIONS[partId];
            if (snapPosition) {
                node.x(snapPosition.x * width);
                node.y(snapPosition.y * height);
            }
            //Uzliek rotāciju un mērogu
            applyPartTransform(partId, node);
        });
        //Notīra vecās pozīcijas un atjaunina positionTracker ar jaunajām
        positionTracker.positions = {};
        Object.keys(PARTS).forEach(partId => positionTracker.trackPosition(partId));
        //Ja ir izvēlēta daļa, atjaunina transformer rīku
        if (selectedPartId) {
            transformer.forceUpdate();
        }
        //pārveido kanvu
        layer.draw();
    }


    //Ielādē saglabāto snap izkārtojumu no datubāzes un uzliek to uz kanvas
    async function loadSavedSnapLayout() {
        if (!buildId) {
            return false;
        }

        try {
            //Veic GET pieprasījumu uz serveri, lai iegūtu saglabāto izkārtojumu
            const response = await fetch(`/DroneBuilder/GetAssemblyLayout?id=${encodeURIComponent(buildId)}`, {
                method: 'GET',
                headers: {
                    'Accept': 'application/json'
                }
            });
            //Pārbauda, vai atbilde ir veiksmīga
            if (!response.ok) {
                throw new Error(`Failed to load layout (${response.status})`);
            }
            //Ja nav datu vai nav layoutJson, izbeidzam
            const result = await response.json();
            if (!result || !result.layoutJson) {
                return false;
            }


            //Parsē JSON tekstu par objektu
            const layout = JSON.parse(result.layoutJson);
            //Uzliek saglabātās pozīcijas un transformācijas
            applySnapLayout(layout);
            //Ja kanvā jau ir komponentes, uzliek izkārtojumu vizuāli
            if (layer.find('.part').length > 0) {
                applySnapLayoutToCanvas();
            }
            console.log('✓ Saved snap layout loaded from database');
            return true;
        } catch (error) {
            console.error('✗ Failed to load saved snap layout', error);
            return false;
        }
    }


    //Izmanto, lai parādītu, vai izkārtojums ir saglabāts vai vēl nav
    function setSaveLayoutButtonState(text, isSaved) {
        if (!saveLayoutButton) {
            return;
        }

        //Maina redzamo tekstu uz pogas
        saveLayoutButton.textContent = text;
        //Pārslēdz CSS klasi 'is-saved'
        saveLayoutButton.classList.toggle('is-saved', !!isSaved);
    }

    //Saglabā pašreizējo snap izkārtojumu datubāzē
    async function saveCurrentLayout() {
        if (!buildId) {
            setSaveLayoutButtonState('Save Failed', false);
            console.error('✗ Failed to save layout: build id missing');
            return;
        }

        try {
            //Parāda lietotājam, ka saglabāšana ir procesā
            setSaveLayoutButtonState('Saving...', false);
            //Iegūst pašreizējo izkārtojumu
            const layout = getCurrentSnapLayout();
            //Nosūta datus uz serveri
            const response = await fetch('/DroneBuilder/SaveAssemblyLayout', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Accept': 'application/json'
                },
                body: JSON.stringify({
                    buildId: buildId,
                    layoutJson: JSON.stringify(layout)
                })
            });


            //Pārbauda, vai atbilde veiksmīgi
            if (!response.ok) {
                throw new Error(`Failed to save layout (${response.status})`);
            }

            const result = await response.json();
            //Pārbauda, vai apstiprināta saglabāšana
            if (!result || result.success !== true) {
                throw new Error(result && result.error ? result.error : 'Save failed');
            }


            //Saglabāšana veiksmīga
            hasSavedLayout = true;
            setSaveLayoutButtonState('Saved!', true);
            console.log('✓ Layout saved to database');
            //Pēc 1.5 sekundēm atgriež pogu normālā stāvoklī
            setTimeout(function() {
                setSaveLayoutButtonState('Save Layout', false);
            }, 1500);
        } catch (error) {
            console.error('✗ Failed to save layout', error);
            setSaveLayoutButtonState('Save Failed', false);
        }
    }

    //Atjaunina režīma pārslēgšanas pogas izskatu un stāvokli
    function updateModeToggleUi() {
        if (!modeToggleButton) {
            return;
        }
        //Nosaka, vai pašlaik ir mācību režīms
        const isGuideMode = currentMode === 'guide';
        //Pārslēdz CSS klases, lai mainītos pogas izskatu
        modeToggleButton.classList.toggle('is-guide', isGuideMode);
        modeToggleButton.classList.toggle('is-developer', !isGuideMode);
        //Piešķir aria-pressed atribūtu pieejamībai
        modeToggleButton.setAttribute('aria-pressed', isGuideMode ? 'true' : 'false');
        //Atjaunina iekšējo tekstu 
        const state = modeToggleButton.querySelector('.mode-toggle-state');
        if (state) {
            state.textContent = isGuideMode ? 'Guide' : 'Developer';
        }
    }


    //Atjaunina izstrādātāja režīma kontroļu redzamību
    function updateDeveloperControlsVisibility() {
        //Nosaka, vai pašlaik ir izstrādātāja režīms
        const shouldShowDeveloperControls = currentMode === 'developer';
        //Slēpj vai rāda "Save Layout" pogu
        if (saveLayoutButton) {
            saveLayoutButton.classList.toggle('is-hidden', !shouldShowDeveloperControls);
        }
        //Slēpj vai rāda Snap Toggle pogu
        if (snapToggleButton) {
            snapToggleButton.classList.toggle('is-hidden', !shouldShowDeveloperControls);
        }
    }


    //Uzņem pašreizējo kanvas stāvokli 
    function captureCanvasSnapshot() {
        const snapshot = {};


        //Iet cauri visām komponentēm un saglabā to pašreizējos parametrus
        Object.keys(PARTS).forEach(partId => {
            const node = getPartNode(partId);
            if (!node) {
                return;
            }


            //Saglabā svarīgākos parametrus par katru komponenti
            snapshot[partId] = {
                x: node.x(),
                y: node.y(),
                visible: node.visible(),
                draggable: node.draggable(),
                rotation: node.rotation(),
                scaleX: node.scaleX(),
                scaleY: node.scaleY()
            };
        });

        return snapshot;
    }


    //Atjauno kanvas stāvokli no iepriekš saglabāta snapshot
    function restoreCanvasSnapshot(snapshot) {
        if (!snapshot) {
            return;
        }


        // Iet cauri visām komponetēm snapshot objektā un atjauno to parametrus
        Object.keys(snapshot).forEach(partId => {
            //atrod Konva mezglu
            const node = getPartNode(partId);
            //iegūst komponentes konfigurāciju
            const config = PARTS[partId];

            //iegūst saglabāto stāvokli
            const state = snapshot[partId];
            if (!node || !config || !state) {
                return;
            }

            //Aprēķina reālos izmērus pikseļos
            const widthPixels = config.w * width;
            const heightPixels = config.h * height;
            //Uzstāda izmērus un centru
            node.width(widthPixels);
            node.height(heightPixels);
            node.offsetX(widthPixels / 2);
            node.offsetY(heightPixels / 2);
            //Atjauno pozīciju, redzamību, vilkšanas iespēju un transformācijas
            node.x(state.x);
            node.y(state.y);
            node.visible(state.visible);
            node.draggable(state.draggable);
            node.rotation(state.rotation);
            node.scaleX(state.scaleX);
            node.scaleY(state.scaleY);
        });


        //Notīra vecās pozīcijas un atjaunina positionTracker ar jaunajiem datiem
        positionTracker.positions = {};
        Object.keys(PARTS).forEach(partId => positionTracker.trackPosition(partId));
        //Parāda rīku un atjaunina to, ja ir izvēlēta daļa
        transformer.visible(true);
        if (selectedPartId) {
            transformer.forceUpdate();
        }
        //pārveido kanvu, lai visas izmaiņas būtu redzamas
        layer.draw();
    }

    //Aprēķina ierobežotu vilkšanas pozīciju, lai daļa neizietu ārpus kanvas robežām
    function getClampedDragPosition(node, pos) {
        //Iegūst komponentes izmēru un pozīciju, ignorējot stroke un shadow
        const rect = node.getClientRect({ skipStroke: true, skipShadow: true });

        //jaunā X pozīcija, ko lietotājs mēģina uzstādīt
        let nextX = pos.x;   
        //jaunā Y pozīcija
        let nextY = pos.y;   

        //Iekšējā funkcija, kas pārbauda un koriģē pozīciju, lai tā paliktu robežās
        const applyBounds = function () {
            //cik tālu cenšas pārvietot pa X
            const deltaX = nextX - node.x();
            //cik tālu cenšas pārvietot pa Y
            const deltaY = nextY - node.y();      

            //Aprēķina, kur nonāktu daļas malas pēc pārvietošanas
            const shiftedLeft = rect.x + deltaX;
            const shiftedTop = rect.y + deltaY;
            const shiftedRight = shiftedLeft + rect.width;
            const shiftedBottom = shiftedTop + rect.height;

            //Ja daļa iziet pa kreiso malu → koriģē atpakaļ
            if (shiftedLeft < 0) {
                nextX += -shiftedLeft;
            }

            //Ja daļa iziet pa labo malu → koriģē atpakaļ
            if (shiftedRight > width) {
                nextX -= shiftedRight - width;
            }

            //Ja daļa iziet pa augšējo malu → koriģē atpakaļ
            if (shiftedTop < 0) {
                nextY += -shiftedTop;
            }

            //Ja daļa iziet pa apakšējo malu → koriģē atpakaļ
            if (shiftedBottom > height) {
                nextY -= shiftedBottom - height;
            }
        };

        //Izpilda korekciju divas reizes, lai nodrošinātu precīzu rezultātu
        applyBounds();
        applyBounds();

        return { x: nextX, y: nextY };
    }

    //Ja tāds mācību solis neeksistē, atgriež null
    function getGuideStep(index) {
        return GUIDE_STEPS[index] || null;
    }

    //Pārbauda, vai mācību solis ir pabeigts
    function isGuideStepComplete(index) {
        const step = getGuideStep(index);
        if (!step) {
            return false;
        }

        //Solis ir pabeigts, ja autoComplete ir piepildijies vai atzīmēts teorijas guideCompletedSteps kā pabeigtssolis
        return !!step.autoComplete || guideCompletedSteps.has(index);
    }

    //Pārbauda, vai komponente ir snap pozīcijā - tuvu saglabātajai vietai
    function isPartSnapped(partId) {
        const node = getPartNode(partId);
        if (!node) return false;

        const group = getInterchangeableGroup(partId);

        //Ja ir grupa – pārbauda visus grupas locekļus
        if (group) {
            return group.some(memberId => {
                const snapPos = SNAP_POSITIONS[memberId];
                if (!snapPos) return false;

                const targetX = snapPos.x * width;
                const targetY = snapPos.y * height;

                return Math.hypot(node.x() - targetX, node.y() - targetY) <= Math.max(8, getSnapThreshold() / 4);
            });
        }

        //Vienkārša pārbaude vienai daļai
        const snapPosition = SNAP_POSITIONS[partId];
        if (!snapPosition) return false;

        const targetX = snapPosition.x * width;
        const targetY = snapPosition.y * height;

        return Math.hypot(node.x() - targetX, node.y() - targetY) <= Math.max(8, getSnapThreshold() / 4);
    }
    //Novieto komponenti precīzi pozīcijā
    function positionPartAtSnap(partId) {
        const node = getPartNode(partId);
        const config = PARTS[partId];
        const snapPosition = SNAP_POSITIONS[partId];
        if (!node || !config || !snapPosition) {
            return;
        }

        //Iegūst komponentu izmērus pikseļos
        const pix = getPixels(config);
        //Uzstāda izmērus un centru
        node.width(pix.w);
        node.height(pix.h);
        node.offsetX(pix.w / 2);
        node.offsetY(pix.h / 2);
        //Uzliek precīzu snap pozīciju
        node.x(snapPosition.x * width);
        node.y(snapPosition.y * height);
        //Piemēro rotāciju un mērogu no PART_TRANSFORMS
        applyPartTransform(partId, node);
    }
    //Novieto daļu sākotnējā pozīcijā mācibu režīmam
    function positionPartAtGuideStart(partId) {
        const node = getPartNode(partId);
        const config = PARTS[partId];
        if (!node || !config) {
            return;
        }
        //Iegūst sākotnējos izmērus un pozīciju
        const pix = getInitialPixels(partId, config);
        //Uzstāda izmērus un centru, lai rotācija un mērogs darbotos pareizi
        node.width(pix.w);
        node.height(pix.h);
        node.offsetX(pix.w / 2);
        node.offsetY(pix.h / 2);
        //Uzliek sākotnējo pozīciju
        node.x(pix.x);
        node.y(pix.y);
        //Piemēro rotāciju un mērogu
        applyPartTransform(partId, node);
    }
    // Piemēro mācību režīma stāvokli uz visām komponentēm
    function applyGuideModeState(resetCurrentParts) {
        const currentStep = getGuideStep(currentGuideStepIndex);
        //Saglabā to komponentes ID, kuras jau ir pabeigtas un jābūt redzamām
        const completedVisibleParts = new Set(['frame']);
        //Iet cauri visiem iepriekšējiem soļiem un pievieno pabeigtās daļas
        GUIDE_STEPS.forEach((step, stepIndex) => {
            if (stepIndex >= currentGuideStepIndex) {
                return;
            }

            if (isGuideStepComplete(stepIndex)) {
                (step.partIds || []).forEach(partId => completedVisibleParts.add(partId));
            }
        });
        //Ja pašreizējais solis ir pabeigts, pievieno arī tā komponentes kā pabeigtas
        if (currentStep && isGuideStepComplete(currentGuideStepIndex)) {
            (currentStep.partIds || []).forEach(partId => completedVisibleParts.add(partId));
        }
        //Apstrādā katru komponentu atsevišķi
        Object.entries(PARTS).forEach(([partId, config]) => {
            const node = getPartNode(partId);
            if (!node) {
                return;
            }

            const isCurrentPart = currentStep && !isGuideStepComplete(currentGuideStepIndex) && currentStep.partIds.includes(partId);
            const isCompletedPart = completedVisibleParts.has(partId);

            if (isCompletedPart) {
                //Pabeigtās daļas redzamas, bet nevar vilkt, nostiprinātas snap pozīcijā
                node.visible(true);
                node.draggable(false);
                positionPartAtSnap(partId);
                return;
            }

            if (isCurrentPart) {
                //Pašreizējā soļa komponente ir redzama un var vilkt
                node.visible(true);
                node.draggable(!!config.draggable);
                //Ja nepieciešama reset vai komponente vēl nav sākta
                if (resetCurrentParts || !guideStartedParts.has(partId)) {
                    positionPartAtGuideStart(partId);
                    guideStartedParts.add(partId);
                }
                return;
            }
            //Visas pārējās komponentes, mācību režīmā ir paslēptas
            node.visible(false);
            node.draggable(false);
        });

        //Notīra atlasi un transformer mācību režīmā
        selectPart(null);
        transformer.visible(false);
        //Noņem visus apmales efektus
        layer.find('.part').forEach(function (node) {
            node.stroke('none');
            node.strokeWidth(0);
        });
        //Atjaunina positionTracker un pārtaisa kanvu
        positionTracker.positions = {};
        Object.keys(PARTS).forEach(partId => positionTracker.trackPosition(partId));
        //Parāda vizuālos palīgelementus
        showGuideHighlights();
        layer.draw();
    }
    //Atjaunina navigācijas elementus
    function updateGuideNavigation() {
        //Atjaunina soļa numuru
        if (stepCounterElement) {
            stepCounterElement.textContent = `${currentGuideStepIndex + 1} / ${GUIDE_STEPS.length}`;
        }

        //Atspējo Iepriekšējā soļa pogu, ja ir pirmais solis
        if (prevStepButton) {
            prevStepButton.disabled = currentGuideStepIndex === 0;
        }

        // Atspējo Nākamā soļa pogu, ja ir pēdējais solis vai pašreizējais solis vēl nav pabeigts
        if (nextStepButton) {
            nextStepButton.disabled = currentGuideStepIndex >= GUIDE_STEPS.length - 1 ||
                !isGuideStepComplete(currentGuideStepIndex);
        }
    }
    


    //Atjaunina paneļus Developer režīmā
    function renderDeveloperModePanels() {
        if (stepListElement) {
            stepListElement.innerHTML = '<p class="placeholder-text">Switch to Guide mode to follow the drone build step by step.</p>';
        }
        if (stepCounterElement) {
            stepCounterElement.textContent = 'Developer mode';
        }
        if (prevStepButton) {
            prevStepButton.disabled = true;
        }
        if (nextStepButton) {
            nextStepButton.disabled = true;
        }
    }

    //Atzīmē pašreizējo soli kā pabeigtu un automātiski pāriet uz nākamo
    function completeCurrentGuideStep(autoAdvance) {
        guideCompletedSteps.add(currentGuideStepIndex);

        //Notīra automātisko pārejas taimeri, ja tāds bija aktīvs
        if (guideAutoAdvanceHandle) {
            window.clearTimeout(guideAutoAdvanceHandle);
            guideAutoAdvanceHandle = null;
        }

        //Ja autoAdvance ir true un nav pēdējais solis – pāriet uz nākamo soli pēc 180ms
        if (autoAdvance && currentGuideStepIndex < GUIDE_STEPS.length - 1) {
            guideAutoAdvanceHandle = window.setTimeout(function () {
                currentGuideStepIndex += 1;
                applyGuideModeState(true);
                renderGuideModePanels();
            }, 180);
            return;
        }

        //Parastais pabeigšanas ceļš
        applyGuideModeState(false);
        renderGuideModePanels();
    }

    //Izveido un atjaunina soļu sarakstu
    function renderGuideStepList() {
        if (!stepListElement) {
            return;
        }

        //Aprēķina, cik soļi ir atslēgti
        const maxUnlockedIndex = Math.min(
            GUIDE_STEPS.length - 1,
            currentGuideStepIndex + (isGuideStepComplete(currentGuideStepIndex) ? 1 : 0)
        );

        //Ģenerē HTML katram solim
        stepListElement.innerHTML = GUIDE_STEPS.map((step, index) => {
            //Sagatavo CSS klases katram solim
            const classes = ['step-item'];
            if (index === currentGuideStepIndex) classes.push('active');
            if (isGuideStepComplete(index)) classes.push('completed');
            if (index > maxUnlockedIndex) classes.push('locked');

            //Ja solim ir action poga un tas ir pašreizējais nepabeigtais solis – pievieno pogu
            const actionHtml = step.isAction && index === currentGuideStepIndex && !isGuideStepComplete(index)
                ? `<div class="step-action-wrap"><button type="button" class="button small guide-step-action">${step.actionLabel || 'Mark Step Complete'}</button></div>`
                : '';
            //Atgriež HTML vienu soli
            return `
            <div class="${classes.join(' ')}" data-step-index="${index}">
                <div class="step-num">${index + 1}</div>
                <div class="step-text">
                    <div class="step-title">${step.title}</div>
                    <div class="step-description">${step.description}</div>
                    ${actionHtml}
                </div>
            </div>`;
        }).join('');

        //Pievieno klikšķu apstrādi soļu sarakstam
        stepListElement.querySelectorAll('[data-step-index]').forEach(item => {
            item.addEventListener('click', function (e) {
                if (e.target.closest('.guide-step-action')) return;

                // Iegūst mērķa soļa indeksu no data atribūta
                const targetIndex = Number(this.getAttribute('data-step-index'));
                //Aprēķina, cik soļus lietotājs drīkst pāriet
                const maxIndex = Math.min(
                    GUIDE_STEPS.length - 1,
                    currentGuideStepIndex + (isGuideStepComplete(currentGuideStepIndex) ? 1 : 0)
                );
                //Ja mēģina pāriet uz vēl neatbloķētu soli – ignorē klikšķi
                if (targetIndex > maxIndex) return;

                //Maina pašreizējo soli un atjaunina visu mācību režīmu
                currentGuideStepIndex = targetIndex;
                applyGuideModeState(true);
                renderGuideModePanels();
            });
        });

        //Pievieno klikšķu apstrādi "Mark Step Complete" pogām
        stepListElement.querySelectorAll('.guide-step-action').forEach(button => {
            button.addEventListener('click', function () {
                completeCurrentGuideStep(true);
            });
        });
    }

    //Atjaunina sarakstu un navigāciju
    function renderGuideModePanels() {
        renderGuideStepList();
        updateGuideNavigation();
    }

    //Inicializē mācību režīmu no nulles
    function initializeGuideMode() {
        currentGuideStepIndex = 0;
        guideCompletedSteps = new Set();
        guideStartedParts = new Set();

        //Saglabā iepriekšējo Snap iestatījumu no izstrādātāja režīma
        developerSnapEnabledBeforeGuide = isSnapEnabled;

        setSnapEnabled(true);
        setDebugVisible(false);

        applyGuideModeState(true);
        renderGuideModePanels();
    }

    //Pārslēdz starp mācību un izstrādātāja režīmu
    function switchMode(mode) {
        if (mode === currentMode) return;

        if (mode === 'guide') {
            //Saglabā pašreizējo kanvas stāvokli pirms pārejas uz mācību režīma
            developerCanvasSnapshot = captureCanvasSnapshot();

            currentMode = 'guide';
            updateModeToggleUi();
            updateDeveloperControlsVisibility();
            initializeGuideMode();
            return;
        }

        //Pāreja atpakaļ uz izstrādātāja režīmu
        currentMode = 'developer';
        clearGuideHighlights();

        updateModeToggleUi();
        updateDeveloperControlsVisibility();

        setSnapEnabled(developerSnapEnabledBeforeGuide);
        setDebugVisible(true);
        transformer.visible(true);

        //Atjauno kanvas stāvokli, kāds bija pirms mācību režīma
        restoreCanvasSnapshot(developerCanvasSnapshot);

        //Atjauno stroke efektus izstrādātāja režīmā
        layer.find('.part').forEach(function (node) {
            var config = PARTS[node.id()];
            if (config && config.draggable) {
                node.stroke('rgba(100, 150, 200, 0.2)');
                node.strokeWidth(1);
            }
        });

        renderDeveloperModePanels();
    }

    //Ātrā pārslēgšanās starp režīmiem
    function toggleMode() {
        switchMode(currentMode === 'developer' ? 'guide' : 'developer');
    }

    //Automātiski pārbauda, vai pašreizējais solis ir pabeigts
    function evaluateGuideStepCompletion() {
        if (currentMode !== 'guide') return;

        const step = getGuideStep(currentGuideStepIndex);
        if (!step || step.isAction || step.autoComplete) return;

        //Pārbauda, vai VISAS komponentes šajā solī ir snap pozīcijā
        const isComplete = step.partIds.every(partId => isPartSnapped(partId));

        if (!isComplete) {
            showGuideHighlights();
            layer.draw();
            return;
        }

        //Ja viss ir kārtībā pabeidz soli un pāriet tālāk
        completeCurrentGuideStep(true);
    }
    

    var guideHighlightNodes = [];
    var guideHighlightAnim = null;

    //Notīra visus mācību highlight elementus un aptur animāciju
    function clearGuideHighlights() {
        if (guideHighlightAnim) {
            guideHighlightAnim.stop();
            guideHighlightAnim = null;
        }

        guideHighlightNodes.forEach(function (node) {
            node.destroy();
        });

        guideHighlightNodes = [];
    }

    //Parāda animētus mērķa apļus pašreizējam solim
    function showGuideHighlights() {
        //notīra vecos apļus
        clearGuideHighlights();

        //Ja nav mācību režīmā – neko nedara
        if (currentMode !== 'guide') {
            return;
        }

        const step = getGuideStep(currentGuideStepIndex);
        if (!step || step.isAction || isGuideStepComplete(currentGuideStepIndex) || step.partIds.length === 0) {
            return;
        }
        //mērķa pozīcijas, kur rādīt highlightus
        const targets = [];   
        //lai neparādītu vairākas reizes vienu un to pašu grupu
        const processedGroups = new Set();     

        //Iet cauri visām komponentēm, kas jānovieto šajā solī
        step.partIds.forEach(function (partId) {
            const group = getInterchangeableGroup(partId);

            //Apstrādā grupu
            if (group) {
                const groupKey = group.join(',');
                if (processedGroups.has(groupKey)) return;
                processedGroups.add(groupKey);

                group.forEach(function (memberId) {
                    const snapPos = SNAP_POSITIONS[memberId];
                    if (!snapPos) return;

                    const tX = snapPos.x * width;
                    const tY = snapPos.y * height;

                    //Pārbauda, vai kāda no grupas komponentēm jau ir novietota šajā pozīcijā
                    const occupied = group.some(function (otherId) {
                        const otherNode = getPartNode(otherId);
                        if (!otherNode || !otherNode.visible()) return false;
                        return Math.hypot(otherNode.x() - tX, otherNode.y() - tY) <= Math.max(8, getSnapThreshold() / 4);
                    });

                    //Ja pozīcija ir brīva – pievieno apļa mērķi
                    if (!occupied) {
                        targets.push({ x: snapPos.x, y: snapPos.y, partId: memberId });
                    }
                });
                return;
            }

            //Parasta pārbaudē, ja jau ir pareizajā vietā, apli nevajag
            if (isPartSnapped(partId)) {
                return;
            }

            const snapPos = SNAP_POSITIONS[partId];
            if (snapPos) {
                targets.push({ x: snapPos.x, y: snapPos.y, partId: partId });
            }
        });

        //Izveido vizuālos apļus katram mērķim
        targets.forEach(function (target) {
            const ring = new Konva.Circle({
                x: target.x * width,
                y: target.y * height,
                radius: 60,
                stroke: 'rgba(220, 38, 38, 0.8)',
                strokeWidth: 2,
                fill: 'rgba(220, 38, 38, 0.25)',
                listening: false,
                name: 'guide-highlight'
            });

            layer.add(ring);
            guideHighlightNodes.push(ring);
        });

        //Izvirza pašreizējās komponentes priekšplānā
        step.partIds.forEach(function (partId) {
            const node = getPartNode(partId);
            if (node && node.visible()) {
                node.moveToTop();
            }
        });

        //Pulsējošu animāciju
        if (guideHighlightNodes.length > 0) {
            guideHighlightAnim = new Konva.Animation(function (frame) {
                const cycle = (frame.time % 1800) / 1800;
                const opacity = 0.35 + 0.65 * (0.5 + 0.5 * Math.sin(cycle * 2 * Math.PI));

                guideHighlightNodes.forEach(function (node) {
                    node.opacity(opacity);
                });
            }, layer);

            guideHighlightAnim.start();
        }
    }

    //Izveido Konva.Transformer objektu – rīku, kas ļauj lietotājam vilkt, mainīt izmēru un rotēt daļas
    const transformer = new Konva.Transformer({
        //ieslēdz rotācijas rokturi
        rotateEnabled: true,   
        //kuri izmēru maiņas rokturi ir aktīvi
        enabledAnchors: [                       
            'top-left', 'top-center', 'top-right',
            'middle-left', 'middle-right',
            'bottom-left', 'bottom-center', 'bottom-right'
        ],
        //ļauj mainīt izmēru brīvi
        keepRatio: false,    
        //ignorē stroke biezumu, aprēķinot bounding box
        ignoreStroke: true,   
        //transformera apmales krāsa
        borderStroke: 'rgba(71, 211, 229, 0.95)', 
        //apmales biezums
        borderStrokeWidth: 2,     
        //rokturu apmales krāsa
        anchorStroke: '#1f6feb', 
        //rokturu iekšējā krāsa (balta)
        anchorFill: '#ffffff',    
        //rokturu izmērs pikseļos
        anchorSize: 10,   
        //attālums no centra līdz rotācijas rokturim
        rotateAnchorOffset: 22,     
        //mērogošana no centra (false = no malas)
        centeredScaling: false,                 

        //Ierobežo minimālo izmēru, lai daļa nekļūtu pārāk maza
        boundBoxFunc: function (oldBox, newBox) {
            if (Math.abs(newBox.width) < 16 || Math.abs(newBox.height) < 16) {
                //Ja jaunais izmērs ir pārāk mazs – atceļ izmaiņas
                return oldBox;   
            }
            return newBox;
        }
    });

    //Pievieno transformer
    layer.add(transformer);

    //Sākotnēji transformer ir paslēpts
    transformer.visible(false);

    //Aprēķina komponentu pozīciju un izmērus pikseļos
    function getPixels(config) {
        const snapPosition = SNAP_POSITIONS[config.id];
        const x = snapPosition ? snapPosition.x : config.x;
        const y = snapPosition ? snapPosition.y : config.y;

        return {
            x: x * width,
            y: y * height,
            w: config.w * width,
            h: config.h * height
        };
    }

    //Aprēķina sākotnējo pozīciju un izmērus daļai mācību režīmā
    function getInitialPixels(partId, config) {
        const size = {
            w: config.w * width,
            h: config.h * height
        };

        //Nepārvietojamām komponentes novieto tieši snap pozīcijā
        if (!config.draggable) {
            const placed = getPixels(config);
            return {
                x: placed.x,
                y: placed.y,
                w: size.w,
                h: size.h
            };
        }

        //Pārvietojamās komponentes izkārto sākuma pozīcijās apakšā
        const index = DRAGGABLE_PART_IDS.indexOf(partId);
        const totalParts = DRAGGABLE_PART_IDS.length;
        const rowCount = BOTTOM_START_ROWS.length;
        const columns = Math.ceil(totalParts / rowCount);
        const row = Math.floor(index / columns);
        const col = index % columns;

        const itemsInRow = row === rowCount - 1
            ? totalParts - (columns * (rowCount - 1)) || columns
            : columns;

        const startX = width * START_SIDE_PADDING_RATIO;
        const endX = width * (1 - START_SIDE_PADDING_RATIO);
        const slotWidth = itemsInRow > 0 ? (endX - startX) / itemsInRow : (endX - startX);

        const rawX = startX + (slotWidth * col) + (slotWidth / 2);
        const rawY = height * (BOTTOM_START_ROWS[row] ?? BOTTOM_START_ROWS[BOTTOM_START_ROWS.length - 1]);

        const clamped = clampToCanvas(rawX, rawY, size.w, size.h);

        return {
            x: clamped.x,
            y: clamped.y,
            w: size.w,
            h: size.h
        };
    }

    //Atgriež snap attāluma slieksni
    function getSnapThreshold() {
        return Math.min(width, height) * SNAP_THRESHOLD_RATIO;
    }

    //Atjaunina Snap Toggle pogas izskatu un tekstu
    function updateSnapToggleUi() {
        if (!snapToggleButton) return;

        snapToggleButton.classList.toggle('is-on', isSnapEnabled);
        snapToggleButton.classList.toggle('is-off', !isSnapEnabled);
        snapToggleButton.setAttribute('aria-pressed', isSnapEnabled ? 'true' : 'false');

        const state = snapToggleButton.querySelector('.snap-toggle-state');
        if (state) {
            state.textContent = isSnapEnabled ? 'On' : 'Off';
        }
    }

    //Iestata snap funkcionalitāti ieslēgtu/izslēgtu
    function setSnapEnabled(enabled) {
        isSnapEnabled = !!enabled;
        updateSnapToggleUi();
        console.log(`🧲 Snap positions ${isSnapEnabled ? 'enabled' : 'disabled'}`);
        return isSnapEnabled;
    }

    //Ātri pārslēdz snap režīmu
    function toggleSnapEnabled() {
        return setSnapEnabled(!isSnapEnabled);
    }

    //Automātiski "pielīmē" daļu pie tuvākās snap pozīcijas
    function snapPartToTarget(partId, partNode, onSnapped) {
        if (!isSnapEnabled) {
            return false;
        }

        const group = getInterchangeableGroup(partId);

        //Apstrāde grupām
        if (group) {
            let bestTargetX = null;
            let bestTargetY = null;
            let bestDistance = Infinity;

            group.forEach(function (memberId) {
                const snapPos = SNAP_POSITIONS[memberId];
                if (!snapPos) return;

                const tX = snapPos.x * width;
                const tY = snapPos.y * height;
                const dist = Math.hypot(partNode.x() - tX, partNode.y() - tY);

                if (dist <= getSnapThreshold() && dist < bestDistance) {
                    const occupied = group.some(function (otherId) {
                        if (otherId === partId) return false;
                        const otherNode = getPartNode(otherId);
                        if (!otherNode || !otherNode.visible()) return false;
                        return Math.hypot(otherNode.x() - tX, otherNode.y() - tY) <= Math.max(8, getSnapThreshold() / 4);
                    });

                    if (!occupied) {
                        bestDistance = dist;
                        bestTargetX = tX;
                        bestTargetY = tY;
                    }
                }
            });

            if (bestTargetX === null) return false;

            //Animēta pārvietošana uz snap pozīciju
            partNode.to({
                x: bestTargetX,
                y: bestTargetY,
                duration: 0.2,
                easing: Konva.Easings.EaseOut,
                onFinish: function () {
                    positionTracker.trackPosition(partId);
                    if (selectedPartId === partId) {
                        transformer.forceUpdate();
                    }
                    if (typeof onSnapped === 'function') onSnapped();
                    layer.draw();
                }
            });

            return true;
        }

        //Parastā snap loģika vienai komponentei
        const snapPosition = SNAP_POSITIONS[partId];
        if (!snapPosition) return false;

        const targetX = snapPosition.x * width;
        const targetY = snapPosition.y * height;
        const distance = Math.hypot(partNode.x() - targetX, partNode.y() - targetY);

        if (distance > getSnapThreshold()) {
            return false;
        }

        partNode.to({
            x: targetX,
            y: targetY,
            duration: 0.2,
            easing: Konva.Easings.EaseOut,
            onFinish: function () {
                positionTracker.trackPosition(partId);
                if (selectedPartId === partId) {
                    transformer.forceUpdate();
                }
                if (typeof onSnapped === 'function') onSnapped();
                layer.draw();
            }
        });

        return true;
    }

    //Asinhroni ielādē attēlu un atgriež Promise
    function loadImageAsync(src) {
        return new Promise((resolve, reject) => {
            const img = new Image();
            img.onload = () => resolve(img);
            img.onerror = () => reject(new Error(`Failed to load: ${src}`));
            img.src = src;
        });
    }

    //kanvas ārējā robeža
    let borderRect = null; 
    //horizontālā viduslīnija
    let hLine = null; 
    //vertikālā viduslīnija
    let vLine = null; 
    //centra punkts
    let centerDot = null;    

    //Iestata debug elementu redzamību
    function setDebugVisible(visible) {
        if (borderRect) borderRect.visible(visible);
        if (hLine) hLine.visible(visible);
        if (vLine) vLine.visible(visible);
        if (centerDot) centerDot.visible(visible);
    }

    //Izveido debug robežas, viduslīnijas un centra punktu
    function addDebugInfo() {
        borderRect = new Konva.Rect({
            x: 0,
            y: 0,
            width: width,
            height: height,
            stroke: 'rgba(71, 211, 229, 0.35)',
            strokeWidth: 1,
            dash: [6, 6],
            listening: false
        });

        hLine = new Konva.Line({
            points: [0, height / 2, width, height / 2],
            stroke: 'rgba(71, 211, 229, 0.3)',
            strokeWidth: 1,
            dash: [6, 6],
            listening: false
        });

        vLine = new Konva.Line({
            points: [width / 2, 0, width / 2, height],
            stroke: 'rgba(71, 211, 229, 0.3)',
            strokeWidth: 1,
            dash: [6, 6],
            listening: false
        });

        centerDot = new Konva.Circle({
            x: width / 2,
            y: height / 2,
            radius: 4,
            fill: 'rgba(71, 211, 229, 0.9)',
            listening: false
        });

        layer.add(borderRect);
        layer.add(hLine);
        layer.add(vLine);
        layer.add(centerDot);
    }

    //Ierobežo pozīciju, lai komponente neizietu ārpus kanvas robežām
    function clampToCanvas(x, y, w, h) {
        return {
            x: Math.max(w / 2, Math.min(width - (w / 2), x)),
            y: Math.max(h / 2, Math.min(height - (h / 2), y))
        };
    }

    //Galvenā funkcija – asinhroni ielādē visas komponentes un pievieno tās kanvai
    async function loadAllParts() {
        console.log('Loading draggable parts with snap targets...');

        updateModeToggleUi();
        updateDeveloperControlsVisibility();
        updateSnapToggleUi();

        //Ielādē katru komponenti pa vienai
        for (const [key, partConfig] of Object.entries(PARTS)) {
            try {
                const img = await loadImageAsync(partConfig.image);
                const pix = getInitialPixels(key, partConfig);

                const konvaImage = new Konva.Image({
                    image: img,
                    x: pix.x,
                    y: pix.y,
                    width: pix.w,
                    height: pix.h,
                    offsetX: pix.w / 2,
                    offsetY: pix.h / 2,
                    draggable: partConfig.draggable,
                    name: 'part',
                    id: key,
                    stroke: partConfig.draggable ? 'rgba(100, 150, 200, 0.2)' : 'none',
                    strokeWidth: 1,
                    dragBoundFunc: function (pos) {
                        return getClampedDragPosition(this, pos);
                    }
                });

                applyPartTransform(key, konvaImage);

                //Klikšķis izvēlas komponenti izstrādātāja režīmā
                konvaImage.on('click tap', function () {
                    if (currentMode !== 'developer') return;
                    selectPart(this);
                });

                konvaImage.on('transformstart', function () {
                    if (currentMode !== 'developer') return;
                    selectPart(this);
                });

                //Kad transformācija beigusies saglabā izmaiņas
                konvaImage.on('transformend', function () {
                    storePartTransform(key, this);
                    const pos = positionTracker.trackPosition(key);
                    if (pos) {
                        console.log(`🛠️ ${pos.partName} transform updated: rotation ${pos.rotation}°, scale (${pos.scaleX}, ${pos.scaleY})`);
                    }
                    transformer.forceUpdate();
                    layer.draw();
                });

                //Pārvietojamām komponenetem pievieno papildu notikumus
                if (partConfig.draggable) {
                    konvaImage.on('dragstart', function () {
                        if (currentMode === 'developer') {
                            selectPart(this);
                            this.stroke('rgba(100, 150, 200, 0.8)');
                            this.strokeWidth(2);
                        }
                        layer.draw();
                    });

                    konvaImage.on('dragend', function () {
                        const snapped = snapPartToTarget(key, this, function () {
                            if (currentMode === 'guide') {
                                evaluateGuideStepCompletion();
                            }
                        });

                        if (!snapped) {
                            const pos = positionTracker.trackPosition(key);
                            console.log(`📍 ${pos.partName} moved to (${pos.percentX}, ${pos.percentY})`);
                        }

                        if (currentMode === 'developer') {
                            this.stroke('rgba(100, 150, 200, 0.2)');
                            this.strokeWidth(1);
                        }
                        layer.draw();
                    });

                    //Maina kursoru, kad virs daļas
                    konvaImage.on('mouseover', function () {
                        document.body.style.cursor = 'grab';
                    });

                    konvaImage.on('mouseout', function () {
                        document.body.style.cursor = 'default';
                    });

                    konvaImage.on('mousedown', function () {
                        document.body.style.cursor = 'grabbing';
                    });

                    konvaImage.on('mouseup', function () {
                        document.body.style.cursor = 'grab';
                    });
                }

                layer.add(konvaImage);
                console.log(`✓ ${key}: ${pix.w.toFixed(0)}×${pix.h.toFixed(0)}px`);

            } catch (err) {
                console.error(`✗ ${key}:`, err.message);
            }
        }

        //Klikšķis uz tukšas vietas vai ārpus daļām – atceļ atlasi
        stage.on('click tap', function (e) {
            const clickedOnTransformer = e.target === transformer || e.target.getParent() === transformer;
            if (e.target === stage || (!clickedOnTransformer && !e.target.hasName('part'))) {
                selectPart(null);
            }
        });

        //Ja ir saglabāts izkārtojums – uzliek to
        if (hasSavedLayout) {
            applySnapLayoutToCanvas();
        }

        //Inicializē positionTracker ar visām komponenetēs
        Object.keys(PARTS).forEach(partId => {
            positionTracker.trackPosition(partId);
        });

        layer.draw();
        console.log('All parts loaded!\n');

        //Pēc ielādes pārslēdzas uz pareizo režīmu
        if (currentMode === 'guide') {
            developerCanvasSnapshot = captureCanvasSnapshot();
            initializeGuideMode();
        } else {
            renderDeveloperModePanels();
        }
    }

    //Apstrādā loga izmēra maiņu, pārrēķinot visu komponentu pozīcijas un izmērus
    window.addEventListener('resize', () => {
        const oldWidth = width;
        const oldHeight = height;

        //Iegūst jauno kanvas izmēru
        ({ width, height } = getContentSize(workbenchArea));

        stage.width(width);
        stage.height(height);

        //Pārrēķina un pārvieto visas daļas, saglabājot to relatīvo pozīciju
        layer.find('.part').forEach(node => {
            const config = PARTS[node.id()];
            if (!config) return;

            const pctX = node.x() / oldWidth;
            const pctY = node.y() / oldHeight;
            const newW = config.w * width;
            const newH = config.h * height;

            node.x(pctX * width);
            node.y(pctY * height);
            node.width(newW);
            node.height(newH);
            node.offsetX(newW / 2);
            node.offsetY(newH / 2);
        });

        //Atjaunina debug elementus
        if (borderRect) {
            borderRect.width(width);
            borderRect.height(height);
        }
        if (hLine) hLine.points([0, height / 2, width, height / 2]);
        if (vLine) vLine.points([width / 2, 0, width / 2, height]);
        if (centerDot) {
            centerDot.x(width / 2);
            centerDot.y(height / 2);
        }

        if (selectedPartId) {
            transformer.forceUpdate();
        }

        if (currentMode === 'guide') {
            showGuideHighlights();
        }

        layer.draw();
    });

    console.log('=== SNAP ASSEMBLY GUIDE ===');
    console.log('All parts are draggable and snap near their saved targets!');
    console.log('Base frame is your reference (not draggable).\n');

    //Galvenā inicializācijas funkcija
    async function initializeAssembly() {
        //Ielādē saglabāto izkārtojumu no datubāzes
        hasSavedLayout = await loadSavedSnapLayout();

        addDebugInfo();

        //Mācību režīmā debug elementi ir paslēpti
        if (currentMode === 'guide') {
            setDebugVisible(false);
        }

        //Ielādē visas komponentes un pievieno tās kanvai
        await loadAllParts();
    }

    //Palaiž inicializāciju
    initializeAssembly();

    //Režīma pārslēgšanas poga
    if (modeToggleButton) {
        modeToggleButton.addEventListener('click', function () {
            toggleMode();
        });
    }

    //Iepriekšējais solis
    if (prevStepButton) {
        prevStepButton.addEventListener('click', function () {
            if (currentMode !== 'guide' || currentGuideStepIndex === 0) return;

            currentGuideStepIndex -= 1;
            applyGuideModeState(true);
            renderGuideModePanels();
        });
    }

    //Nākamais solis
    if (nextStepButton) {
        nextStepButton.addEventListener('click', function () {
            if (currentMode !== 'guide' ||
                currentGuideStepIndex >= GUIDE_STEPS.length - 1 ||
                !isGuideStepComplete(currentGuideStepIndex)) {
                return;
            }

            currentGuideStepIndex += 1;
            applyGuideModeState(true);
            renderGuideModePanels();
        });
    }

    //Saglabāšanas poga
    if (saveLayoutButton) {
        saveLayoutButton.addEventListener('click', function () {
            saveCurrentLayout();
        });
    }

    //Snap ieslēgšanas/izslēgšanas poga
    if (snapToggleButton) {
        snapToggleButton.addEventListener('click', function () {
            toggleSnapEnabled();
        });
    }

    window.positionTracker = positionTracker;

    window.partEditor = {
        select: function (partId) {
            const node = getPartNode(partId);
            if (!node) {
                console.warn(`Part not found: ${partId}`);
                return null;
            }
            selectPart(node);
            return node;
        },
        rotate: setPartRotation,
        scale: setPartScale,
        log: logPartState
    };

    window.snapControls = {
        isEnabled: function () { return isSnapEnabled; },
        on: function () { return setSnapEnabled(true); },
        off: function () { return setSnapEnabled(false); },
        toggle: toggleSnapEnabled
    };

    window.snapLayoutControls = {
        save: saveCurrentLayout,
        load: loadSavedSnapLayout,
        export: getCurrentSnapLayout
    };

    window.assemblyModeControls = {
        current: function () { return currentMode; },
        developer: function () { switchMode('developer'); return currentMode; },
        guide: function () { switchMode('guide'); return currentMode; },
        toggle: toggleMode
    };

    //Piemēro saglabāto rotāciju un mērogu komponentei
    function applyPartTransform(partId, partNode) {
        const transform = getPartTransform(partId);
        partNode.rotation(transform.rotation);
        partNode.scaleX(transform.scaleX);
        partNode.scaleY(transform.scaleY);
    }

    //Atgriež daļas transformācijas datus
    function getPartTransform(partId) {
        return PART_TRANSFORMS[partId] || { rotation: 0, scaleX: 1, scaleY: 1 };
    }

    //Saglabā pašreizējo rotāciju un mērogu
    function storePartTransform(partId, partNode) {
        PART_TRANSFORMS[partId] = {
            rotation: Number(partNode.rotation().toFixed(2)),
            scaleX: Number(partNode.scaleX().toFixed(4)),
            scaleY: Number(partNode.scaleY().toFixed(4))
        };
        return PART_TRANSFORMS[partId];
    }

    //Atrod Konva mezglu pēc partId
    function getPartNode(partId) {
        return layer.findOne(`#${partId}`);
    }

    // zvēlas komponentu un aktivizē transformer
    function selectPart(partNode) {
        selectedPartId = partNode ? partNode.id() : null;
        transformer.nodes(partNode ? [partNode] : []);
        transformer.moveToTop();
        layer.draw();
    }

    //Izdrukā komponentu pašreizējo stāvokli konsolē
    function logPartState(partId) {
        const pos = positionTracker.getPosition(partId);
        if (!pos) {
            console.warn(`Part not found: ${partId}`);
            return null;
        }
        console.log(pos);
        return pos;
    }

    //Uzstāda komponentu rotāciju
    function setPartRotation(partId, degrees) {
        const node = getPartNode(partId);
        if (!node) return null;

        node.rotation(degrees);
        storePartTransform(partId, node);
        positionTracker.trackPosition(partId);

        if (selectedPartId === partId) {
            transformer.forceUpdate();
        }
        layer.draw();
        return logPartState(partId);
    }

    //Uzstāda komponentu mērogu
    function setPartScale(partId, scaleX, scaleY) {
        const node = getPartNode(partId);
        if (!node) return null;

        node.scaleX(scaleX);
        node.scaleY(scaleY);
        storePartTransform(partId, node);
        positionTracker.trackPosition(partId);

        if (selectedPartId === partId) {
            transformer.forceUpdate();
        }
        layer.draw();
        return logPartState(partId);
    }
});
