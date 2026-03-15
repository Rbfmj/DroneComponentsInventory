document.addEventListener('DOMContentLoaded', function () {
    // Add full-width class to body for layout override
    document.body.classList.add('builder-page');

    const selects = document.querySelectorAll('.category-select');
    const btnSave = document.getElementById('btn-save');
    const resultsDiv = document.getElementById('compatibility-results');
    const summaryDiv = document.getElementById('build-summary');
    const saveResult = document.getElementById('save-result');

    function getVal(id) {
        const el = document.getElementById(id);
        return el && el.value ? parseInt(el.value) : null;
    }

    function getSelection() {
        return {
            frameId: getVal('select-frame'),
            motorId: getVal('select-motor'),
            propellerId: getVal('select-propeller'),
            escId: getVal('select-esc'),
            batteryId: getVal('select-battery'),
            fcId: getVal('select-fc'),
            cameraId: getVal('select-camera'),
            vtxId: getVal('select-vtx'),
            videoAntennaId: getVal('select-videoantenna'),
            receiverId: getVal('select-receiver'),
            receiverAntennaId: getVal('select-receiverantenna'),
            radioControllerId: getVal('select-radiocontroller'),
            fpvGogglesId: getVal('select-fpvgoggles')
        };
    }

    function getSelectedText(id) {
        const el = document.getElementById(id);
        if (!el || !el.value) return null;
        return el.options[el.selectedIndex].text;
    }

    function updateSummary() {
        const categories = [
            { id: 'select-frame', label: 'Frame' },
            { id: 'select-motor', label: 'Motors (x4)' },
            { id: 'select-propeller', label: 'Propellers (x4)' },
            { id: 'select-esc', label: 'ESC' },
            { id: 'select-battery', label: 'Battery' },
            { id: 'select-fc', label: 'Flight Controller' },
            { id: 'select-camera', label: 'FPV Camera' },
            { id: 'select-vtx', label: 'Video Transmitter' },
            { id: 'select-videoantenna', label: 'Video Antenna' },
            { id: 'select-receiver', label: 'Receiver' },
            { id: 'select-receiverantenna', label: 'Receiver Antenna' },
            { id: 'select-radiocontroller', label: 'Radio Controller' },
            { id: 'select-fpvgoggles', label: 'FPV Goggles' }
        ];

        let hasSelection = false;
        let html = '<h6 style="margin-bottom:0.75rem;font-weight:600;">Selected Components</h6>';

        categories.forEach(function (cat) {
            const text = getSelectedText(cat.id);
            if (text) {
                hasSelection = true;
                html += '<div class="summary-card">' +
                    '<div class="label">' + cat.label + '</div>' +
                    '<div class="value">' + text + '</div>' +
                    '</div>';
            }
        });

        if (!hasSelection) {
            html = '<p class="placeholder-text">Select components from the left panel...</p>';
        }

        summaryDiv.innerHTML = html;
    }

    async function checkCompatibility() {
        const selection = getSelection();
        const hasAny = Object.values(selection).some(function (v) { return v !== null; });

        if (!hasAny) {
            resultsDiv.innerHTML = '<p class="placeholder-text">Select at least one component</p>';
            return;
        }

        resultsDiv.innerHTML = '<p class="placeholder-text">Checking...</p>';

        try {
            const response = await fetch('/DroneBuilder/CheckCompatibility', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(selection)
            });
            const results = await response.json();
            displayResults(results);
        } catch (err) {
            resultsDiv.innerHTML = '<p style="color:#dc3545;font-size:0.8rem;">Check failed: ' + err.message + '</p>';
        }
    }

    function displayResults(results) {
        var statusOrder = { critical: 0, warning: 1, ok: 2, unchecked: 3 };

        results.sort(function (a, b) {
            var sa = statusOrder[a.status] !== undefined ? statusOrder[a.status] : 3;
            var sb = statusOrder[b.status] !== undefined ? statusOrder[b.status] : 3;
            if (sa !== sb) return sa - sb;
            return b.priority - a.priority;
        });

        // Count statuses
        var counts = { ok: 0, critical: 0, warning: 0, unchecked: 0 };
        results.forEach(function (r) { counts[r.status] = (counts[r.status] || 0) + 1; });

        var html = '<div class="status-summary">' +
            '<span class="status-count status-count-ok">OK: ' + counts.ok + '</span>' +
            '<span class="status-count status-count-critical">Critical: ' + counts.critical + '</span>' +
            '<span class="status-count status-count-warning">Warning: ' + counts.warning + '</span>' +
            '<span class="status-count status-count-unchecked">N/A: ' + counts.unchecked + '</span>' +
            '</div>';

        results.forEach(function (r) {
            var label = 'N/A';
            var cls = 'check-unchecked';
            if (r.status === 'ok') { label = 'OK'; cls = 'check-ok'; }
            else if (r.status === 'critical') { label = 'FAIL'; cls = 'check-critical'; }
            else if (r.status === 'warning') { label = 'WARN'; cls = 'check-warning'; }

            html += '<div class="check-item ' + cls + '">' +
                '<strong>[' + label + '] #' + r.checkNumber + ' ' + r.name + '</strong>' +
                '<p>' + r.message + '</p>' +
                '</div>';
        });

        resultsDiv.innerHTML = html;
    }

    // Highlight selected dropdowns & trigger real-time check
    selects.forEach(function (s) {
        s.addEventListener('change', function () {
            if (this.value) {
                this.classList.add('has-selection');
            } else {
                this.classList.remove('has-selection');
            }
            updateSummary();
            checkCompatibility();
        });
    });

    btnSave.addEventListener('click', async function () {
        var name = document.getElementById('build-name').value.trim();
        if (!name) {
            saveResult.innerHTML = '<p style="color:#dc3545;font-size:0.8rem;">Please enter a name!</p>';
            return;
        }

        var selection = getSelection();
        selection.name = name;

        try {
            var response = await fetch('/DroneBuilder/SaveBuild', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(selection)
            });
            var result = await response.json();

            if (result.success) {
                saveResult.innerHTML = '<p style="color:#198754;font-size:0.8rem;">Saved! (ID: ' + result.buildId + ')</p>';
            } else {
                saveResult.innerHTML = '<p style="color:#dc3545;font-size:0.8rem;">Error: ' + (result.error || 'Unknown') + '</p>';
            }
        } catch (err) {
            saveResult.innerHTML = '<p style="color:#dc3545;font-size:0.8rem;">Error: ' + err.message + '</p>';
        }
    });
});
