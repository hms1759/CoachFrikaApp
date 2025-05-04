
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

        });
    }
});