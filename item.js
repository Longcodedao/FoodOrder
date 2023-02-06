const openModalButtons = document.querySelectorAll("[data-modal-target]");
const closeModalButtons = document.querySelectorAll("[data-close-button]");
const overlay = document.getElementById("overlay");

openModalButtons.forEach((button) => {
  button.addEventListener("click", () => {
    const modal = document.querySelector(button.dataset.modalTarget);
    openModal(modal);
  });
});

overlay.addEventListener("click", () => {
  const modals = document.querySelectorAll(".modal.active");
  modals.forEach((modal) => {
    closeModal(modal);
  });
});

closeModalButtons.forEach((button) => {
  button.addEventListener("click", () => {
    const modal = button.closest(".modal");
    closeModal(modal);
  });
});

function openModal(modal) {
  if (modal == null) return;
  modal.classList.add("active");
  overlay.classList.add("active");
}

function closeModal(modal) {
  if (modal == null) return;
  modal.classList.remove("active");
  overlay.classList.remove("active");
}
let current_rating = document.querySelector(".current_rating");
let review_item_name = document.querySelector(".review-item-name");
let item_name = document.querySelector(".item-name");

review_item_name.innerText = `${item_name.innerText}`;
const allStars = document.querySelectorAll(".star");
allStars.forEach((star, i) => {
  star.onclick = function () {
    let current_star_level = i + 1;
    current_rating.innerText = `${current_star_level} out of 5`;
    allStars.forEach((star, j) => {
      if (current_star_level >= j + 1) {
        star.innerHTML = "&#9733";
      } else {
        star.innerHTML = "&#9734";
      }
    });
  };
});
