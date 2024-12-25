// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

<script>
    var deleteModal = document.getElementById('deleteModal');
    deleteModal.addEventListener('show.bs.modal', function (event) {
        // Tetikleyen butonu al
        var button = event.relatedTarget;

    // Butondan veri-rol-id ve veri-rol-name değerlerini al
    var roleId = button.getAttribute('data-role-id');
    var roleName = button.getAttribute('data-role-name');

    // Modal içindeki elemanlara bu değerleri ata
    var roleNameElement = deleteModal.querySelector('#roleName');
    var roleIdInput = deleteModal.querySelector('#roleId');

    roleNameElement.textContent = roleName;
    roleIdInput.value = roleId;
    });

    document.getElementById("hamburger").addEventListener("click", function () {
        this.classList.toggle("active");
    document.querySelector(".nav-links").classList.toggle("active");
});


</script>

// Hamburger menu toggle


