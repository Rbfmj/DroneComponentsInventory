(function () {

    /* ---------------------------------------------
       Open modal when a video antennas tile is clicked
       --------------------------------------------- */
    document
        .getElementById('videoantennasGrid')
        ?.addEventListener('click', event => {

            const tile = event.target.closest('[data-modal-id]');
            if (!tile) return;

            const modal = document.getElementById(tile.dataset.modalId);
            if (!modal) return;

            modal.style.display = 'flex';
            modal.setAttribute('aria-hidden', 'false');
            document.body.style.overflow = 'hidden';
        });

    /* ---------------------------------------------
       Close modal via close buttons (delegated)
       --------------------------------------------- */
    document.addEventListener('click', event => {
        if (!event.target.classList.contains('close-modal')) return;

        const modal = event.target.closest('.videoantennas-modal');
        if (modal) closeModal(modal);
    });

    /* ---------------------------------------------
       Click outside panel to close modal
       --------------------------------------------- */
    document.querySelectorAll('.videoantennas-modal').forEach(modal => {
        modal.addEventListener('click', event => {
            if (event.target === modal) {
                closeModal(modal);
            }
        });
    });

    /* ---------------------------------------------
       Close modal with ESC key
       --------------------------------------------- */
    document.addEventListener('keydown', event => {
        if (event.key !== 'Escape') return;

        const openModal = document.querySelector(
            '.videoantennas-modal[style*="flex"]'
        );

        if (openModal) closeModal(openModal);
    });

    /* ---------------------------------------------
       Centralized modal close logic
       --------------------------------------------- */
    function closeModal(modal) {
        modal.style.display = 'none';
        modal.setAttribute('aria-hidden', 'true');
        document.body.style.overflow = '';
    }

})();