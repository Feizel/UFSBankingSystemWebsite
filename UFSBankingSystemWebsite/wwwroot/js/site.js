$(document).ready(function () {

    // Sidebar toggle functionality
    $('#sidebarCollapse').on('click', function () {
        $('#sidebar').toggleClass('active');
        const isActive = $('#sidebar').hasClass('active');
        $(this).attr('aria-expanded', isActive);

        // Optionally adjust the main content area when sidebar is toggled
        $('#content').toggleClass('active');

        // Adjust the button icon based on the sidebar state
        $(this).find('i').toggleClass('fa-bars fa-times');

        // Optional animation effect for smooth transitions
        $('#content').fadeToggle(300);

        // Add ripple effect to buttons (if desired)
        $('.btn').on('click', function (e) {
            let x = e.clientX - e.target.offsetLeft;
            let y = e.clientY - e.target.offsetTop;

            let ripple = document.createElement('span');
            ripple.style.left = `${x}px`;
            ripple.style.top = `${y}px`;
            ripple.classList.add('ripple');

            this.appendChild(ripple);

            setTimeout(() => {
                ripple.remove();
            }, 600);
        });

        // Password strength indicator
        const passwordInput = $('#Password');
        const passwordStrengthIndicator = $('#passwordStrength');

        passwordInput.on('input', function () {
            const password = $(this).val();
            let strength = 0;

            // Check password length
            if (password.length >= 8) strength++;
            if (/[A-Z]/.test(password) && /[a-z]/.test(password)) strength++;
            if (/\d/.test(password)) strength++;
            if (/[\W_]/.test(password)) strength++;

            // Update strength indicator
            let strengthText = '';
            switch (strength) {
                case 0:
                    strengthText = 'Very Weak';
                    passwordStrengthIndicator.css('color', 'red');
                    break;
                case 1:
                    strengthText = 'Weak';
                    passwordStrengthIndicator.css('color', 'orange');
                    break;
                case 2:
                    strengthText = 'Moderate';
                    passwordStrengthIndicator.css('color', 'yellow');
                    break;
                case 3:
                    strengthText = 'Strong';
                    passwordStrengthIndicator.css('color', 'lightgreen');
                    break;
                case 4:
                    strengthText = 'Very Strong';
                    passwordStrengthIndicator.css('color', 'green');
                    break;
            }

            passwordStrengthIndicator.text(`Password Strength: ${strengthText}`);
        });

        // Animate cards on page load (optional)
        gsap.from('.card', { duration: 0.5, opacity: 0, y: 50, stagger: 0.1, ease: 'power2.out' });

        // Animate sidebar items on hover (optional)
        $('.sidebar-item').hover(
            function () { gsap.to(this, { duration: 0.3, x: 10, ease: 'power2.out' }); },
            function () { gsap.to(this, { duration: 0.3, x: 0, ease: 'power2.out' }); }
        );

    });
});