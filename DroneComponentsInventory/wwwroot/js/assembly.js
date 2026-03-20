// Assembly Guide - FREE ROAM WITH COORDINATE TRACKING
// All zones removed, free movement, position tracking

document.addEventListener('DOMContentLoaded', function () {
    var build = window.DRONE_BUILD;
    const modeToggleButton = document.getElementById('modeToggleButton');
    const snapToggleButton = document.getElementById('snapToggleButton');
    const saveLayoutButton = document.getElementById('saveLayoutButton');
    const stepListElement = document.getElementById('step-list');
    const stepCounterElement = document.getElementById('step-counter');
    const prevStepButton = document.getElementById('btn-prev');
    const nextStepButton = document.getElementById('btn-next');
    const componentInfoElement = document.getElementById('component-info');
    const SNAP_LAYOUT_STORAGE_KEY = 'droneAssemblySnapLayout';
    let saveLayoutButtonResetHandle = null;
    let isSnapEnabled = true;
    let currentMode = 'developer';
    let currentGuideStepIndex = 0;
    let guideCompletedSteps = new Set();
    let guideStartedParts = new Set();
    let developerCanvasSnapshot = null;
    let developerSnapEnabledBeforeGuide = true;

    // ================================================================
    //  KONVA STAGE SETUP
    // ================================================================
    const container = document.getElementById('container');
    if (!container) {
        console.error('Container #container not found');
        return;
    }

    const workbenchArea = container.parentElement;

    function getContentSize(el) {
        const s = window.getComputedStyle(el);
        return {
            width:  el.clientWidth  - parseFloat(s.paddingLeft)  - parseFloat(s.paddingRight),
            height: el.clientHeight - parseFloat(s.paddingTop)   - parseFloat(s.paddingBottom)
        };
    }

    let { width, height } = getContentSize(workbenchArea);

    console.log(`Canvas size: ${width}x${height}`);

    const stage = new Konva.Stage({
        container: 'container',
        width: width,
        height: height
    });

    const layer = new Konva.Layer();
    stage.add(layer);

    // ================================================================
    //  PART DEFINITIONS (with initial positions)
    // ================================================================
    const PARTS = {
        'frame': {
            image: '/images/parts/frame.png',
            x: 0.5, y: 0.5, w: 0.8, h: 0.8,
            draggable: false,
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
            image: '/images/parts/motor.svg',
            x: 0.25, y: 0.25, w: 0.1, h: 0.1,
            draggable: true,
            name: 'MOTOR 1',
            id: 'motor-fl'
        },
        'motor-fr': {
            image: '/images/parts/motor.svg',
            x: 0.75, y: 0.25, w: 0.1, h: 0.1,
            draggable: true,
            name: 'MOTOR 2',
            id: 'motor-fr'
        },
        'motor-bl': {
            image: '/images/parts/motor.svg',
            x: 0.25, y: 0.75, w: 0.1, h: 0.1,
            draggable: true,
            name: 'MOTOR 3',
            id: 'motor-bl'
        },
        'motor-br': {
            image: '/images/parts/motor.svg',
            x: 0.75, y: 0.75, w: 0.1, h: 0.1,
            draggable: true,
            name: 'MOTOR 4',
            id: 'motor-br'
        },
        'fc': {
            image: '/images/parts/fc-esc.svg',
            x: 0.5, y: 0.5, w: 0.12, h: 0.12,
            draggable: true,
            name: 'FLIGHT CONTROLLER',
            id: 'fc'
        },
        'capacitor': {
            image: '/images/parts/cap-lead.svg',
            x: 0.35, y: 0.5, w: 0.08, h: 0.08,
            draggable: true,
            name: 'CAPACITOR',
            id: 'capacitor'
        },
        'battery-lead': {
            image: '/images/parts/cap-lead.svg',
            x: 0.65, y: 0.5, w: 0.08, h: 0.08,
            draggable: true,
            name: 'BATTERY LEAD',
            id: 'battery-lead'
        },
        'camera': {
            image: '/images/parts/fpvcamera.svg',
            x: 0.5, y: 0.15, w: 0.09, h: 0.07,
            draggable: true,
            name: 'CAMERA',
            id: 'camera'
        },
        'receiver': {
            image: '/images/parts/receiver.svg',
            x: 0.5, y: 0.72, w: 0.1, h: 0.1,
            draggable: true,
            name: 'RECEIVER',
            id: 'receiver'
        },
        'vtx': {
            image: '/images/parts/vtx-antenna.svg',
            x: 0.5, y: 0.85, w: 0.1, h: 0.1,
            draggable: true,
            name: 'VTX + ANTENNA',
            id: 'vtx'
        },
        'propeller-fl': {
            image: '/images/parts/propeller.svg',
            x: 0.075, y: 0.075, w: 0.15, h: 0.15,
            draggable: true,
            name: 'PROPELLER 1',
            id: 'propeller-fl'
        },
        'propeller-fr': {
            image: '/images/parts/propeller.svg',
            x: 0.925, y: 0.075, w: 0.15, h: 0.15,
            draggable: true,
            name: 'PROPELLER 2',
            id: 'propeller-fr'
        },
        'propeller-bl': {
            image: '/images/parts/propeller.svg',
            x: 0.075, y: 0.925, w: 0.15, h: 0.15,
            draggable: true,
            name: 'PROPELLER 3',
            id: 'propeller-bl'
        },
        'propeller-br': {
            image: '/images/parts/propeller.svg',
            x: 0.925, y: 0.925, w: 0.15, h: 0.15,
            draggable: true,
            name: 'PROPELLER 4',
            id: 'propeller-br'
        },
        'battery-plug': {
            image: '/images/parts/battery.svg',
            x: 0.5, y: 0.88, w: 0.08, h: 0.08,
            draggable: true,
            name: 'BATTERY (PLUG)',
            id: 'battery-plug'
        },
        'top-frame': {
            image: '/images/parts/top-frame.png',
            x: 0.5, y: 0.5, w: 0.25, h: 0.25,
            draggable: true,
            name: 'TOP FRAME',
            id: 'top-frame'
        },
        'battery-top': {
            image: '/images/parts/battery.svg',
            x: 0.5, y: 0.5, w: 0.15, h: 0.2,
            draggable: true,
            name: 'BATTERY (TOP)',
            id: 'battery-top'
        }
    };

    const SNAP_POSITIONS = {
        'battery-lead': { x: 0.0403, y: 0.9600 },
        'battery-plug': { x: 0.0400, y: 0.9600 },
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
        'battery-lead': { rotation: 0, scaleX: 1, scaleY: 1 },
        'camera': { rotation: 0, scaleX: 1, scaleY: 1 },
        'receiver': { rotation: 0, scaleX: 1, scaleY: 1 },
        'vtx': { rotation: 0, scaleX: 1, scaleY: 1 },
        'propeller-fl': { rotation: 0, scaleX: 1, scaleY: 1 },
        'propeller-fr': { rotation: 0, scaleX: 1, scaleY: 1 },
        'propeller-bl': { rotation: 0, scaleX: 1, scaleY: 1 },
        'propeller-br': { rotation: 0, scaleX: 1, scaleY: 1 },
        'battery-plug': { rotation: 0, scaleX: 1, scaleY: 1 },
        'top-frame': { rotation: 0, scaleX: 1, scaleY: 1 },
        'battery-top': { rotation: 0, scaleX: 1, scaleY: 1 }
    };

    const SNAP_THRESHOLD_RATIO = 0.08;
    const BOTTOM_START_ROWS = [0.76, 0.91];
    const START_SIDE_PADDING_RATIO = 0.06;

    const DRAGGABLE_PART_IDS = Object.keys(PARTS).filter(partId => PARTS[partId].draggable);

    let selectedPartId = null;

    const GUIDE_STEPS = [
        {
            title: 'Base frame with four arms',
            description: 'Start with the base frame. This is your foundation for the full drone build.',
            partIds: ['frame'],
            autoComplete: true
        },
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
            title: 'Add capacitor and battery lead',
            description: 'Place both the capacitor and battery lead before moving on.',
            partIds: ['capacitor', 'battery-lead']
        },
        {
            title: 'Add all 4 motors',
            description: 'Install all four motors onto the frame arms.',
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
            title: 'Plug in the battery',
            description: 'Connect the battery plug for power-up steps.',
            partIds: ['battery-plug']
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
            description: 'Bind the goggles to the VTX just like the smoke check interaction.',
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
            title: 'Unplug the battery',
            description: 'Disconnect the battery before finishing the build.',
            partIds: [],
            isAction: true,
            actionLabel: 'Mark Battery Unplugged'
        },
        {
            title: 'Add a top frame',
            description: 'Install the top frame plate.',
            partIds: ['top-frame']
        },
        {
            title: 'Add all propellers',
            description: 'Install all four propellers in their correct positions.',
            partIds: ['propeller-fl', 'propeller-fr', 'propeller-bl', 'propeller-br']
        },
        {
            title: 'Install battery on top frame plate',
            description: 'Place the battery onto the top frame plate.',
            partIds: ['battery-top']
        },
        {
            title: 'Congratulations',
            description: 'The drone build is complete.',
            partIds: [],
            isAction: true,
            actionLabel: 'Finish Build'
        }
    ];

    // ================================================================
    //  COORDINATE TRACKING SYSTEM
    // ================================================================
    const positionTracker = {
        // Store current positions in both % and pixels
        positions: {},
        history: [],
        maxHistory: 50,

        // Get current position of a part
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

        // Get all positions
        getAllPositions: function() {
            const allPos = {};
            Object.keys(PARTS).forEach(partId => {
                const pos = this.getPosition(partId);
                if (pos) allPos[partId] = pos;
            });
            return allPos;
        },

        // Track a position change (when part is placed)
        trackPosition: function(partId) {
            const pos = this.getPosition(partId);
            if (!pos) return;

            // Add to history
            this.history.push(pos);
            if (this.history.length > this.maxHistory) {
                this.history.shift();
            }

            // Store as current
            this.positions[partId] = pos;

            return pos;
        },

        // Export as JavaScript object for code
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

        // Export as JSON
        exportAsJSON: function() {
            return JSON.stringify(this.positions, null, 2);
        },

        // Get history of a part
        getHistory: function(partId) {
            return this.history.filter(h => h.partId === partId);
        },

        // Get last position
        getLastPosition: function(partId) {
            const history = this.getHistory(partId);
            return history[history.length - 1] || null;
        },

        // Log all positions to console
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

        // Log as code format
        logAsCode: function() {
            console.clear();
            console.log(this.exportAsCode());
        },

        // Save to localStorage
        saveToLocalStorage: function() {
            localStorage.setItem('dronePositions', this.exportAsJSON());
            console.log('✓ Positions saved to localStorage');
        },

        // Load from localStorage
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

    function getCurrentSnapLayout() {
        const snapPositions = {};
        const partTransforms = {};

        Object.keys(PARTS).forEach(partId => {
            const pos = positionTracker.getPosition(partId);
            if (!pos) {
                return;
            }

            snapPositions[partId] = {
                x: Number(pos.percentX),
                y: Number(pos.percentY)
            };
            partTransforms[partId] = {
                rotation: pos.rotation,
                scaleX: pos.scaleX,
                scaleY: pos.scaleY
            };
        });

        return {
            snapPositions: snapPositions,
            partTransforms: partTransforms,
            savedAt: new Date().toISOString()
        };
    }

    function applySnapLayout(layout) {
        if (!layout) {
            return;
        }

        Object.entries(layout.snapPositions || {}).forEach(([partId, value]) => {
            if (!SNAP_POSITIONS[partId]) {
                SNAP_POSITIONS[partId] = {};
            }

            SNAP_POSITIONS[partId].x = typeof value.x === 'number' ? value.x : SNAP_POSITIONS[partId].x;
            SNAP_POSITIONS[partId].y = typeof value.y === 'number' ? value.y : SNAP_POSITIONS[partId].y;
        });

        Object.entries(layout.partTransforms || {}).forEach(([partId, value]) => {
            PART_TRANSFORMS[partId] = {
                rotation: typeof value.rotation === 'number' ? value.rotation : 0,
                scaleX: typeof value.scaleX === 'number' ? value.scaleX : 1,
                scaleY: typeof value.scaleY === 'number' ? value.scaleY : 1
            };
        });
    }

    function applySnapLayoutToCanvas() {
        Object.keys(PARTS).forEach(partId => {
            const node = getPartNode(partId);
            if (!node) {
                return;
            }

            const snapPosition = SNAP_POSITIONS[partId];
            if (snapPosition) {
                node.x(snapPosition.x * width);
                node.y(snapPosition.y * height);
            }

            applyPartTransform(partId, node);
        });

        positionTracker.positions = {};
        Object.keys(PARTS).forEach(partId => positionTracker.trackPosition(partId));
        if (selectedPartId) {
            transformer.forceUpdate();
        }
        layer.draw();
    }

    function loadSavedSnapLayout() {
        const saved = localStorage.getItem(SNAP_LAYOUT_STORAGE_KEY);
        if (!saved) {
            return false;
        }

        try {
            const layout = JSON.parse(saved);
            applySnapLayout(layout);
            if (layer.find('.part').length > 0) {
                applySnapLayoutToCanvas();
            }
            console.log('✓ Saved snap layout loaded');
            return true;
        } catch (error) {
            console.error('✗ Failed to load saved snap layout', error);
            return false;
        }
    }

    function setSaveLayoutButtonState(text, isSaved) {
        if (!saveLayoutButton) {
            return;
        }

        saveLayoutButton.textContent = text;
        saveLayoutButton.classList.toggle('is-saved', !!isSaved);
    }

    function updateModeToggleUi() {
        if (!modeToggleButton) {
            return;
        }

        const isGuideMode = currentMode === 'guide';
        modeToggleButton.classList.toggle('is-guide', isGuideMode);
        modeToggleButton.classList.toggle('is-developer', !isGuideMode);
        modeToggleButton.setAttribute('aria-pressed', isGuideMode ? 'true' : 'false');

        const state = modeToggleButton.querySelector('.mode-toggle-state');
        if (state) {
            state.textContent = isGuideMode ? 'Guide' : 'Developer';
        }
    }

    function updateDeveloperControlsVisibility() {
        const shouldShowDeveloperControls = currentMode === 'developer';
        if (saveLayoutButton) {
            saveLayoutButton.classList.toggle('is-hidden', !shouldShowDeveloperControls);
        }
        if (snapToggleButton) {
            snapToggleButton.classList.toggle('is-hidden', !shouldShowDeveloperControls);
        }
    }

    function captureCanvasSnapshot() {
        const snapshot = {};

        Object.keys(PARTS).forEach(partId => {
            const node = getPartNode(partId);
            if (!node) {
                return;
            }

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

    function restoreCanvasSnapshot(snapshot) {
        if (!snapshot) {
            return;
        }

        Object.keys(snapshot).forEach(partId => {
            const node = getPartNode(partId);
            const config = PARTS[partId];
            const state = snapshot[partId];
            if (!node || !config || !state) {
                return;
            }

            const widthPixels = config.w * width;
            const heightPixels = config.h * height;
            node.width(widthPixels);
            node.height(heightPixels);
            node.offsetX(widthPixels / 2);
            node.offsetY(heightPixels / 2);
            node.x(state.x);
            node.y(state.y);
            node.visible(state.visible);
            node.draggable(state.draggable);
            node.rotation(state.rotation);
            node.scaleX(state.scaleX);
            node.scaleY(state.scaleY);
        });

        positionTracker.positions = {};
        Object.keys(PARTS).forEach(partId => positionTracker.trackPosition(partId));
        transformer.visible(true);
        if (selectedPartId) {
            transformer.forceUpdate();
        }
        layer.draw();
    }

    function getGuideStep(index) {
        return GUIDE_STEPS[index] || null;
    }

    function isGuideStepComplete(index) {
        const step = getGuideStep(index);
        if (!step) {
            return false;
        }

        return !!step.autoComplete || guideCompletedSteps.has(index);
    }

    function isPartSnapped(partId) {
        const node = getPartNode(partId);
        const snapPosition = SNAP_POSITIONS[partId];
        if (!node || !snapPosition) {
            return false;
        }

        const targetX = snapPosition.x * width;
        const targetY = snapPosition.y * height;
        return Math.hypot(node.x() - targetX, node.y() - targetY) <= Math.max(8, getSnapThreshold() / 4);
    }

    function positionPartAtSnap(partId) {
        const node = getPartNode(partId);
        const config = PARTS[partId];
        const snapPosition = SNAP_POSITIONS[partId];
        if (!node || !config || !snapPosition) {
            return;
        }

        const pix = getPixels(config);
        node.width(pix.w);
        node.height(pix.h);
        node.offsetX(pix.w / 2);
        node.offsetY(pix.h / 2);
        node.x(snapPosition.x * width);
        node.y(snapPosition.y * height);
        applyPartTransform(partId, node);
    }

    function positionPartAtGuideStart(partId) {
        const node = getPartNode(partId);
        const config = PARTS[partId];
        if (!node || !config) {
            return;
        }

        const pix = getInitialPixels(partId, config);
        node.width(pix.w);
        node.height(pix.h);
        node.offsetX(pix.w / 2);
        node.offsetY(pix.h / 2);
        node.x(pix.x);
        node.y(pix.y);
        applyPartTransform(partId, node);
    }

    function applyGuideModeState(resetCurrentParts) {
        const currentStep = getGuideStep(currentGuideStepIndex);
        const completedVisibleParts = new Set(['frame']);

        GUIDE_STEPS.forEach((step, stepIndex) => {
            if (stepIndex >= currentGuideStepIndex) {
                return;
            }

            if (isGuideStepComplete(stepIndex)) {
                (step.partIds || []).forEach(partId => completedVisibleParts.add(partId));
            }
        });

        if (currentStep && isGuideStepComplete(currentGuideStepIndex)) {
            (currentStep.partIds || []).forEach(partId => completedVisibleParts.add(partId));
        }

        Object.entries(PARTS).forEach(([partId, config]) => {
            const node = getPartNode(partId);
            if (!node) {
                return;
            }

            const isCurrentPart = currentStep && !isGuideStepComplete(currentGuideStepIndex) && currentStep.partIds.includes(partId);
            const isCompletedPart = completedVisibleParts.has(partId);

            if (isCompletedPart) {
                node.visible(true);
                node.draggable(false);
                positionPartAtSnap(partId);
                return;
            }

            if (isCurrentPart) {
                node.visible(true);
                node.draggable(!!config.draggable);
                if (resetCurrentParts || !guideStartedParts.has(partId)) {
                    positionPartAtGuideStart(partId);
                    guideStartedParts.add(partId);
                }
                return;
            }

            node.visible(false);
            node.draggable(false);
        });

        selectPart(null);
        transformer.visible(false);
        positionTracker.positions = {};
        Object.keys(PARTS).forEach(partId => positionTracker.trackPosition(partId));
        layer.draw();
    }

    function renderDeveloperModePanels() {
        if (stepListElement) {
            stepListElement.innerHTML = '<p class="placeholder-text">Switch to Guide mode to follow the drone build step by step.</p>';
        }
        if (componentInfoElement) {
            componentInfoElement.innerHTML = '<p class="placeholder-text">Developer mode is active. Drag, snap, resize, rotate, and save the current layout.</p>';
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

    function renderGuideStepList() {
        if (!stepListElement) {
            return;
        }

        const maxUnlockedIndex = Math.min(
            GUIDE_STEPS.length - 1,
            currentGuideStepIndex + (isGuideStepComplete(currentGuideStepIndex) ? 1 : 0)
        );

        stepListElement.innerHTML = GUIDE_STEPS.map((step, index) => {
            const classes = ['step-item'];
            if (index === currentGuideStepIndex) {
                classes.push('active');
            }
            if (isGuideStepComplete(index)) {
                classes.push('completed');
            }
            if (index > maxUnlockedIndex) {
                classes.push('locked');
            }

            return `
                <div class="${classes.join(' ')}" data-step-index="${index}">
                    <div class="step-num">${index + 1}</div>
                    <div class="step-text">${step.title}</div>
                </div>`;
        }).join('');

        stepListElement.querySelectorAll('[data-step-index]').forEach(item => {
            item.addEventListener('click', function() {
                const targetIndex = Number(this.getAttribute('data-step-index'));
                const maxIndex = Math.min(
                    GUIDE_STEPS.length - 1,
                    currentGuideStepIndex + (isGuideStepComplete(currentGuideStepIndex) ? 1 : 0)
                );

                if (targetIndex > maxIndex) {
                    return;
                }

                currentGuideStepIndex = targetIndex;
                applyGuideModeState(true);
                renderGuideModePanels();
            });
        });
    }

    function renderGuideInfo() {
        if (!componentInfoElement) {
            return;
        }

        const step = getGuideStep(currentGuideStepIndex);
        if (!step) {
            componentInfoElement.innerHTML = '<p class="placeholder-text">Guide step not found.</p>';
            return;
        }

        const partListHtml = (step.partIds || []).length > 0
            ? `<ul>${step.partIds.map(partId => `<li>${PARTS[partId]?.name || partId}</li>`).join('')}</ul>`
            : '<p>No canvas parts are needed for this step.</p>';

        const isComplete = isGuideStepComplete(currentGuideStepIndex);
        const actionHtml = step.isAction
            ? `<button type="button" id="guideActionButton" class="button small fit" ${isComplete ? 'disabled' : ''}>${isComplete ? 'Completed' : (step.actionLabel || 'Mark Step Complete')}</button>`
            : '';

        const statusText = isComplete
            ? 'Completed'
            : step.isAction
                ? 'Complete the action to continue.'
                : 'Drag the required part(s) onto the drone until they snap into place.';

        componentInfoElement.innerHTML = `
            <h6>${step.title}</h6>
            <p>${step.description}</p>
            <p><strong>Status:</strong> ${statusText}</p>
            <div><strong>Required:</strong>${partListHtml}</div>
            ${actionHtml}`;

        const actionButton = document.getElementById('guideActionButton');
        if (actionButton) {
            actionButton.addEventListener('click', function() {
                guideCompletedSteps.add(currentGuideStepIndex);
                applyGuideModeState(false);
                renderGuideModePanels();
            });
        }
    }

    function updateGuideNavigation() {
        if (stepCounterElement) {
            stepCounterElement.textContent = `${currentGuideStepIndex + 1} / ${GUIDE_STEPS.length}`;
        }
        if (prevStepButton) {
            prevStepButton.disabled = currentGuideStepIndex === 0;
        }
        if (nextStepButton) {
            nextStepButton.disabled = currentGuideStepIndex >= GUIDE_STEPS.length - 1 || !isGuideStepComplete(currentGuideStepIndex);
        }
    }

    function renderGuideModePanels() {
        renderGuideStepList();
        renderGuideInfo();
        updateGuideNavigation();
    }

    function initializeGuideMode() {
        currentGuideStepIndex = 0;
        guideCompletedSteps = new Set([0]);
        guideStartedParts = new Set();
        developerSnapEnabledBeforeGuide = isSnapEnabled;
        setSnapEnabled(true);
        applyGuideModeState(true);
        renderGuideModePanels();
    }

    function switchMode(mode) {
        if (mode === currentMode) {
            return;
        }

        if (mode === 'guide') {
            developerCanvasSnapshot = captureCanvasSnapshot();
            currentMode = 'guide';
            updateModeToggleUi();
            updateDeveloperControlsVisibility();
            initializeGuideMode();
            return;
        }

        currentMode = 'developer';
        updateModeToggleUi();
        updateDeveloperControlsVisibility();
        setSnapEnabled(developerSnapEnabledBeforeGuide);
        transformer.visible(true);
        restoreCanvasSnapshot(developerCanvasSnapshot);
        renderDeveloperModePanels();
    }

    function toggleMode() {
        switchMode(currentMode === 'developer' ? 'guide' : 'developer');
    }

    function evaluateGuideStepCompletion() {
        if (currentMode !== 'guide') {
            return;
        }

        const step = getGuideStep(currentGuideStepIndex);
        if (!step || step.isAction || step.autoComplete) {
            return;
        }

        const isComplete = step.partIds.every(partId => isPartSnapped(partId));
        if (!isComplete) {
            return;
        }

        guideCompletedSteps.add(currentGuideStepIndex);
        applyGuideModeState(false);
        renderGuideModePanels();
    }

    // ================================================================
    //  TRANSFORMER SETUP (RESIZE & ROTATE HANDLES)
    // ================================================================
    const transformer = new Konva.Transformer({
        rotateEnabled: true,
        enabledAnchors: [
            'top-left', 'top-center', 'top-right',
            'middle-left', 'middle-right',
            'bottom-left', 'bottom-center', 'bottom-right'
        ],
        keepRatio: false,
        ignoreStroke: true,
        borderStroke: 'rgba(71, 211, 229, 0.95)',
        borderStrokeWidth: 2,
        anchorStroke: '#1f6feb',
        anchorFill: '#ffffff',
        anchorSize: 10,
        rotateAnchorOffset: 22,
        centeredScaling: false,
        boundBoxFunc: function(oldBox, newBox) {
            if (Math.abs(newBox.width) < 16 || Math.abs(newBox.height) < 16) {
                return oldBox;
            }

            return newBox;
        }
    });

    // ================================================================
    //  UTILITY FUNCTIONS
    // ================================================================
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

    function getPartTransform(partId) {
        const transform = PART_TRANSFORMS[partId] || {};

        return {
            rotation: typeof transform.rotation === 'number' ? transform.rotation : 0,
            scaleX: typeof transform.scaleX === 'number' ? transform.scaleX : 1,
            scaleY: typeof transform.scaleY === 'number' ? transform.scaleY : 1
        };
    }

    function applyPartTransform(partId, partNode) {
        const transform = getPartTransform(partId);
        partNode.rotation(transform.rotation);
        partNode.scaleX(transform.scaleX);
        partNode.scaleY(transform.scaleY);
    }

    function storePartTransform(partId, partNode) {
        PART_TRANSFORMS[partId] = {
            rotation: Number(partNode.rotation().toFixed(2)),
            scaleX: Number(partNode.scaleX().toFixed(4)),
            scaleY: Number(partNode.scaleY().toFixed(4))
        };

        return PART_TRANSFORMS[partId];
    }

    function getPartNode(partId) {
        return layer.findOne(`#${partId}`);
    }

    function selectPart(partNode) {
        selectedPartId = partNode ? partNode.id() : null;
        transformer.nodes(partNode ? [partNode] : []);
        transformer.moveToTop();
        layer.draw();
    }

    function logPartState(partId) {
        const pos = positionTracker.getPosition(partId);
        if (!pos) {
            console.warn(`Part not found: ${partId}`);
            return null;
        }

        console.log(pos);
        return pos;
    }

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

    function setPartScale(partId, scaleX, scaleY) {
        const node = getPartNode(partId);
        if (!node) return null;

        node.scaleX(scaleX);
        node.scaleY(typeof scaleY === 'number' ? scaleY : scaleX);
        storePartTransform(partId, node);
        positionTracker.trackPosition(partId);
        if (selectedPartId === partId) {
            transformer.forceUpdate();
        }
        layer.draw();
        return logPartState(partId);
    }

    function resetPartTransform(partId) {
        const node = getPartNode(partId);
        if (!node) return null;

        node.rotation(0);
        node.scaleX(1);
        node.scaleY(1);
        storePartTransform(partId, node);
        positionTracker.trackPosition(partId);
        if (selectedPartId === partId) {
            transformer.forceUpdate();
        }
        layer.draw();
        return logPartState(partId);
    }

    function clampToCanvas(x, y, w, h) {
        const halfW = w / 2;
        const halfH = h / 2;

        return {
            x: Math.max(halfW, Math.min(width - halfW, x)),
            y: Math.max(halfH, Math.min(height - halfH, y))
        };
    }

    function getInitialPixels(partId, config) {
        const size = {
            w: config.w * width,
            h: config.h * height
        };

        if (!config.draggable) {
            const placed = getPixels(config);
            return {
                x: placed.x,
                y: placed.y,
                w: size.w,
                h: size.h
            };
        }

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

    function getSnapThreshold() {
        return Math.min(width, height) * SNAP_THRESHOLD_RATIO;
    }

    function updateSnapToggleUi() {
        if (!snapToggleButton) {
            return;
        }

        snapToggleButton.classList.toggle('is-on', isSnapEnabled);
        snapToggleButton.classList.toggle('is-off', !isSnapEnabled);
        snapToggleButton.setAttribute('aria-pressed', isSnapEnabled ? 'true' : 'false');

        const state = snapToggleButton.querySelector('.snap-toggle-state');
        if (state) {
            state.textContent = isSnapEnabled ? 'On' : 'Off';
        }
    }

    function setSnapEnabled(enabled) {
        isSnapEnabled = !!enabled;
        updateSnapToggleUi();
        console.log(`🧲 Snap positions ${isSnapEnabled ? 'enabled' : 'disabled'}`);
        return isSnapEnabled;
    }

    function toggleSnapEnabled() {
        return setSnapEnabled(!isSnapEnabled);
    }

    function snapPartToTarget(partId, partNode) {
        if (!isSnapEnabled) {
            return false;
        }

        const snapPosition = SNAP_POSITIONS[partId];
        if (!snapPosition) {
            return false;
        }

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
            onFinish: function() {
                const pos = positionTracker.trackPosition(partId);
                if (pos) {
                    console.log(`📌 ${pos.partName} snapped to (${pos.percentX}, ${pos.percentY})`);
                }
                if (selectedPartId === partId) {
                    transformer.forceUpdate();
                }
                layer.draw();
            }
        });

        return true;
    }

    function loadImageAsync(src) {
        return new Promise((resolve, reject) => {
            const img = new Image();
            img.onload = () => resolve(img);
            img.onerror = () => reject(new Error(`Failed to load: ${src}`));
            img.src = src;
        });
    }

    // ================================================================
    //  DRAW DEBUG INFO
    // ================================================================
    let borderRect = null;
    let hLine = null;
    let vLine = null;
    let centerDot = null;

    function addDebugInfo() {
        borderRect = new Konva.Rect({
            x: 0,
            y: 0,
            width: width,
            height: height,
            stroke: 'rgba(0, 0, 0, 0.28)',
            strokeWidth: 2,
            strokeDashArray: [10, 5],
            listening: false,
            name: 'border-guide'
        });
        layer.add(borderRect);

        hLine = new Konva.Line({
            points: [0, height / 2, width, height / 2],
            stroke: 'rgba(255, 0, 0, 0.2)',
            strokeWidth: 1,
            listening: false,
            name: 'debug'
        });
        layer.add(hLine);

        vLine = new Konva.Line({
            points: [width / 2, 0, width / 2, height],
            stroke: 'rgba(255, 0, 0, 0.2)',
            strokeWidth: 1,
            listening: false,
            name: 'debug'
        });
        layer.add(vLine);

        centerDot = new Konva.Circle({
            x: width / 2,
            y: height / 2,
            radius: 5,
            fill: 'rgba(255, 0, 0, 0.3)',
            listening: false,
            name: 'debug'
        });
        layer.add(centerDot);
        layer.add(transformer);
    }

    // ================================================================
    //  LOAD ALL PARTS WITH KONVA (WITH SNAP TARGETS)
    // ================================================================
    async function loadAllParts() {
        console.log('Loading draggable parts with snap targets...');

        updateModeToggleUi();
        updateDeveloperControlsVisibility();
        updateSnapToggleUi();

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

                    // ===== BOUNDARY CLAMPING =====
                    dragBoundFunc: function(pos) {
                        const halfW = (this.width() * Math.abs(this.scaleX())) / 2;
                        const halfH = (this.height() * Math.abs(this.scaleY())) / 2;
                        const minX = halfW;
                        const maxX = width - halfW;
                        const minY = halfH;
                        const maxY = height - halfH;

                        let newX = pos.x;
                        let newY = pos.y;

                        if (newX < minX) newX = minX;
                        if (newX > maxX) newX = maxX;
                        if (newY < minY) newY = minY;
                        if (newY > maxY) newY = maxY;

                        return { x: newX, y: newY };
                    }
                });

                applyPartTransform(key, konvaImage);

                konvaImage.on('click tap', function() {
                    if (currentMode !== 'developer') {
                        return;
                    }
                    selectPart(this);
                });

                konvaImage.on('transformstart', function() {
                    if (currentMode !== 'developer') {
                        return;
                    }
                    selectPart(this);
                });

                konvaImage.on('transformend', function() {
                    storePartTransform(key, this);
                    const pos = positionTracker.trackPosition(key);
                    if (pos) {
                        console.log(`🛠️ ${pos.partName} transform updated: rotation ${pos.rotation}°, scale (${pos.scaleX}, ${pos.scaleY})`);
                    }
                    transformer.forceUpdate();
                    layer.draw();
                });

                // Add drag events if draggable
                if (partConfig.draggable) {
                    konvaImage.on('dragstart', function() {
                        if (currentMode === 'developer') {
                            selectPart(this);
                        }
                        this.stroke('rgba(100, 150, 200, 0.8)');
                        this.strokeWidth(2);
                        layer.draw();
                    });

                    konvaImage.on('dragend', function() {
                        const snapped = snapPartToTarget(key, this);

                        if (!snapped) {
                            const pos = positionTracker.trackPosition(key);
                            console.log(`📍 ${pos.partName} moved to (${pos.percentX}, ${pos.percentY})`);
                        }

                        if (currentMode === 'guide') {
                            evaluateGuideStepCompletion();
                        }
                        
                        this.stroke('rgba(100, 150, 200, 0.2)');
                        this.strokeWidth(1);
                        layer.draw();
                    });

                    konvaImage.on('mouseover', function() {
                        document.body.style.cursor = 'grab';
                    });

                    konvaImage.on('mouseout', function() {
                        document.body.style.cursor = 'default';
                    });

                    konvaImage.on('mousedown', function() {
                        document.body.style.cursor = 'grabbing';
                    });

                    konvaImage.on('mouseup', function() {
                        document.body.style.cursor = 'grab';
                    });
                }

                layer.add(konvaImage);
                console.log(`✓ ${key}: ${pix.w.toFixed(0)}×${pix.h.toFixed(0)}px`);
            } catch (err) {
                console.error(`✗ ${key}:`, err.message);
            }
        }

        stage.on('click tap', function(e) {
            const clickedOnTransformer = e.target === transformer || e.target.getParent() === transformer;
            if (e.target === stage || (!clickedOnTransformer && !e.target.hasName('part'))) {
                selectPart(null);
            }
        });

        // Initialize positions
        Object.keys(PARTS).forEach(partId => {
            positionTracker.trackPosition(partId);
        });

        layer.draw();
        console.log('All parts loaded!\n');
        console.log('💡 Tip: Parts start along the bottom tray area. Drag them near their target, or click a part to resize/rotate it with handles. Use window.positionTracker to track positions.');

        renderDeveloperModePanels();
    }

    // ================================================================
    //  WINDOW RESIZE
    // ================================================================
    window.addEventListener('resize', () => {
        const oldWidth = width;
        const oldHeight = height;
        ({ width, height } = getContentSize(workbenchArea));
        stage.width(width);
        stage.height(height);

        // Scale all parts proportionally to new canvas size
        layer.find('.part').forEach(node => {
            const config = PARTS[node.id()];
            if (!config) return;

            // Convert current pixel position to percentage of old canvas
            const pctX = node.x() / oldWidth;
            const pctY = node.y() / oldHeight;

            // Recalculate size from config percentages
            const newW = config.w * width;
            const newH = config.h * height;

            node.x(pctX * width);
            node.y(pctY * height);
            node.width(newW);
            node.height(newH);
            node.offsetX(newW / 2);
            node.offsetY(newH / 2);
        });

        // Update debug elements
        if (borderRect) { borderRect.width(width); borderRect.height(height); }
        if (hLine) hLine.points([0, height / 2, width, height / 2]);
        if (vLine) vLine.points([width / 2, 0, width / 2, height]);
        if (centerDot) { centerDot.x(width / 2); centerDot.y(height / 2); }
        if (selectedPartId) { transformer.forceUpdate(); }

        layer.draw();
    });

    // ================================================================
    //  INIT
    // ================================================================
    console.log('=== SNAP ASSEMBLY GUIDE ===');
    console.log('All parts are draggable and snap near their saved targets!');
    console.log('Base frame is your reference (not draggable).\n');

    loadSavedSnapLayout();
    
    addDebugInfo();
    loadAllParts();

    if (modeToggleButton) {
        modeToggleButton.addEventListener('click', function() {
            toggleMode();
        });
    }

    if (prevStepButton) {
        prevStepButton.addEventListener('click', function() {
            if (currentMode !== 'guide' || currentGuideStepIndex === 0) {
                return;
            }

            currentGuideStepIndex -= 1;
            applyGuideModeState(true);
            renderGuideModePanels();
        });
    }

    if (nextStepButton) {
        nextStepButton.addEventListener('click', function() {
            if (currentMode !== 'guide' || currentGuideStepIndex >= GUIDE_STEPS.length - 1 || !isGuideStepComplete(currentGuideStepIndex)) {
                return;
            }

            currentGuideStepIndex += 1;
            applyGuideModeState(true);
            renderGuideModePanels();
        });
    }

    if (saveLayoutButton) {
        saveLayoutButton.addEventListener('click', function() {
            saveCurrentLayout();
        });
    }

    if (snapToggleButton) {
        snapToggleButton.addEventListener('click', function() {
            toggleSnapEnabled();
        });
    }

    // Expose position tracker globally
    window.positionTracker = positionTracker;
    window.partEditor = {
        select: function(partId) {
            const node = getPartNode(partId);
            if (!node) {
                console.warn(`Part not found: ${partId}`);
                return null;
            }

            selectPart(node);
            return logPartState(partId);
        },
        get: logPartState,
        setRotation: setPartRotation,
        setScale: setPartScale,
        resetTransform: resetPartTransform,
        exportCode: function() {
            console.clear();
            console.log(positionTracker.exportAsCode());
            return positionTracker.exportAsCode();
        },
        list: function() {
            return Object.keys(PARTS);
        }
    };
    window.snapControls = {
        isEnabled: function() {
            return isSnapEnabled;
        },
        on: function() {
            return setSnapEnabled(true);
        },
        off: function() {
            return setSnapEnabled(false);
        },
        toggle: toggleSnapEnabled
    };
    window.snapLayoutControls = {
        save: saveCurrentLayout,
        load: loadSavedSnapLayout,
        export: getCurrentSnapLayout
    };
    window.assemblyModeControls = {
        current: function() {
            return currentMode;
        },
        developer: function() {
            switchMode('developer');
            return currentMode;
        },
        guide: function() {
            switchMode('guide');
            return currentMode;
        },
        toggle: toggleMode
    };

    console.log('📍 Position Tracking Available!');
    console.log('\nCommands:');
    console.log('  positionTracker.getAllPositions()        - Get all current positions');
    console.log('  positionTracker.getPosition(\'motor-fl\') - Get specific part');
    console.log('  positionTracker.logAllPositions()        - Log all positions, size, and rotation');
    console.log('  positionTracker.logAsCode()              - Log snap + transform code');
    console.log('  positionTracker.exportAsJSON()           - Export as JSON');
    console.log('  positionTracker.exportAsCode()           - Export as code snippet');
    console.log('  partEditor.select(\'camera\')            - Select a part on canvas');
    console.log('  partEditor.setRotation(\'camera\', 15)   - Set rotation in degrees');
    console.log('  partEditor.setScale(\'camera\', 1.2)     - Resize uniformly');
    console.log('  partEditor.setScale(\'camera\', 1.2, 0.9) - Resize width/height independently');
    console.log('  partEditor.resetTransform(\'camera\')    - Reset size and rotation');
    console.log('  partEditor.exportCode()                  - Log paste-ready snap/transform code');
    console.log('  snapControls.on()                        - Turn snap positions on');
    console.log('  snapControls.off()                       - Turn snap positions off');
    console.log('  snapControls.toggle()                    - Toggle snap positions');
    console.log('  snapLayoutControls.save()                - Save current snap layout');
    console.log('  snapLayoutControls.load()                - Load saved snap layout');
    console.log('  snapLayoutControls.export()              - Get current snap layout object');
    console.log('  assemblyModeControls.developer()         - Switch to developer mode');
    console.log('  assemblyModeControls.guide()             - Switch to guide mode');
    console.log('  assemblyModeControls.toggle()            - Toggle developer/guide mode');
    console.log('\nClick any part to resize/rotate it with handles in developer mode, or switch to guide mode to build step by step.');
    console.log('Drag parts from the bottom area toward their targets to snap them, then export them.');
});
