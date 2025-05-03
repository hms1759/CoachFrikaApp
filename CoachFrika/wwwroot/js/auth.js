// JavaScript for Authentication Pages

document.addEventListener('DOMContentLoaded', function () {
    // Tab switching functionality
    const userTypeTabs = document.querySelectorAll('.user-type-btn');

    userTypeTabs.forEach(tab => {
        tab.addEventListener('click', function () {
            userTypeTabs.forEach(t => t.classList.remove('active'));
            this.classList.add('active');
        });
    });

    // Password visibility toggle
    const passwordToggles = document.querySelectorAll('.password-toggle');

    passwordToggles.forEach(toggle => {
        toggle.addEventListener('click', function () {
            const passwordField = this.parentElement.querySelector('input');
            const icon = this.querySelector('i');

            // Toggle password visibility
            if (passwordField.type === 'password') {
                passwordField.type = 'text';
                icon.classList.remove('fa-eye-slash');
                icon.classList.add('fa-eye');
            } else {
                passwordField.type = 'password';
                icon.classList.remove('fa-eye');
                icon.classList.add('fa-eye-slash');
            }
        });
    });

    

    // Create notification system
    function showNotification(message, type) {
        // Create notification element
        const notification = document.createElement('div');
        notification.className = `notification notification-${type}`;
        notification.innerHTML = message;

        // Add to document
        document.body.appendChild(notification);

        // Trigger animation
        setTimeout(() => {
            notification.classList.add('show');
        }, 10);

        // Remove after 3 seconds
        setTimeout(() => {
            notification.classList.remove('show');
            setTimeout(() => {
                document.body.removeChild(notification);
            }, 300);
        }, 3000);
    }

    // Add notification styles
    const notificationStyles = document.createElement('style');
    notificationStyles.innerHTML = `
      .notification {
        position: fixed;
        top: 20px;
        right: 20px;
        padding: 12px 24px;
        background-color: white;
        color: #333;
        border-radius: 4px;
        box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
        z-index: 1000;
        opacity: 0;
        transform: translateY(-10px);
        transition: all 0.3s ease;
      }
      
      .notification.show {
        opacity: 1;
        transform: translateY(0);
      }
      
      .notification-success {
        background-color: #d4edda;
        color: #155724;
        border-left: 4px solid #28a745;
      }
      
      .notification-error {
        background-color: #f8d7da;
        color: #721c24;
        border-left: 4px solid #dc3545;
      }
    `;
    document.head.appendChild(notificationStyles);

    // Handle "Forgot Password" link
    const forgotPasswordLink = document.getElementById('forgotPasswordLink');
    if (forgotPasswordLink) {
        forgotPasswordLink.addEventListener('click', function (e) {
            e.preventDefault();

            // Create modal dynamically
            const modalHTML = `
          <div class="modal fade" id="forgotPasswordModal" tabindex="-1" aria-labelledby="forgotPasswordModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered">
              <div class="modal-content">
                <div class="modal-header">
                  <h5 class="modal-title" id="forgotPasswordModalLabel">Reset Password</h5>
                  <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                  <p>Enter your email address below. We'll send you a link to reset your password.</p>
                  <form id="resetPasswordForm" class="needs-validation" novalidate>
                    <div class="mb-3">
                      <label for="resetEmail" class="form-label">Email Address</label>
                      <input type="email" class="form-control" id="resetEmail" placeholder="sample@gmail.com" required>
                      <div class="invalid-feedback">
                        Please provide a valid email address.
                      </div>
                    </div>
                  </form>
                </div>
                <div class="modal-footer">
                  <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                  <button type="button" class="btn btn-primary" id="sendResetLink">Send Reset Link</button>
                </div>
              </div>
            </div>
          </div>
        `;

            // Append modal to body
            const modalContainer = document.createElement('div');
            modalContainer.innerHTML = modalHTML;
            document.body.appendChild(modalContainer);

            // Initialize and show modal
            const forgotPasswordModal = new bootstrap.Modal(document.getElementById('forgotPasswordModal'));
            forgotPasswordModal.show();

            // Handle send reset link button
            document.getElementById('sendResetLink').addEventListener('click', function () {
                const resetForm = document.getElementById('resetPasswordForm');

                if (!resetForm.checkValidity()) {
                    resetForm.classList.add('was-validated');
                    return;
                }

                const email = document.getElementById('resetEmail').value;
                console.log('Password reset requested for:', email);

                // Show success message
                forgotPasswordModal.hide();
                showNotification('Password reset link sent! Check your email.', 'success');

                // Clean up modal after hiding
                document.body.removeChild(modalContainer);
            });
        });
    }
});