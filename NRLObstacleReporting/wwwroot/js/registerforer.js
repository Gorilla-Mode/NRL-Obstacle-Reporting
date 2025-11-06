
        // ===== Status-knapp i tabellen =====
        function toggleMenu(button) {
        const td = button.closest('td');
        const menu = td.querySelector('.status-menu');
        menu.classList.toggle('hidden');
    }

        function setStatus(button, status) {
        const td = button.closest('td');
        const statusBtn = td.querySelector('.status-btn');
        statusBtn.textContent = status;
        statusBtn.classList.remove('bg-yellow-200', 'bg-green-200', 'bg-red-200', 'text-yellow-800', 'text-green-800', 'text-red-800');
        if (status === 'Pending') statusBtn.classList.add('bg-yellow-200', 'text-yellow-800');
        else if (status === 'Approved') statusBtn.classList.add('bg-green-200', 'text-green-800');
        else if (status === 'Rejected') statusBtn.classList.add('bg-red-200', 'text-red-800');
        button.closest('.status-menu').classList.add('hidden');
    }

        // ===== Filter-knapper =====
        function toggleFilterDropdown(button) {
        const parent = button.parentElement;
        const menu = parent.querySelector('.filter-menu');
        menu.classList.toggle('hidden');
    }

        function setFilter(item) {
        const parent = item.closest('.relative');
        const button = parent.querySelector('.filter-btn');
        button.textContent = item.textContent;
        parent.querySelector('.filter-menu').classList.add('hidden');
    }
        // This is an anonymous (lambda) function used as a click event handler.
        // It listens for clicks anywhere in the document and closes all dropdown menus
        document.addEventListener('click', function(e) {
        document.querySelectorAll('.status-menu').forEach(menu => {
            const button = menu.previousElementSibling;
            if (!menu.contains(e.target) && !button.contains(e.target)) menu.classList.add('hidden');
        });
        document.querySelectorAll('.filter-menu').forEach(menu => {
        const button = menu.previousElementSibling;
        if (!menu.contains(e.target) && !button.contains(e.target)) menu.classList.add('hidden');
    });
    });

