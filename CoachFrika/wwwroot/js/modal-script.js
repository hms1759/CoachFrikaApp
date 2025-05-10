const ModalApp = {
    currentStep: 1,
    maxStep: 5, // total number of steps

    next: function () {
        const currentModal = document.getElementById(`modalStep${this.currentStep}`);
        const nextModal = document.getElementById(`modalStep${this.currentStep + 1}`);

        console.log(nextModal);
        if (nextModal) {
            currentModal.classList.remove("modal-visible");
            nextModal.classList.add("modal-visible");
            this.currentStep++;
        } else {
            alert("You’ve completed all the steps!");
            this.close();
        }
    },

    close: function () {
        document.getElementById("modalOverlay").style.display = "none";
    }
};
