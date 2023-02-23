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

const myDiv = document.getElementById("rating-color");

const value = parseFloat(document.getElementById("rating").innerText);

if ((value >= 0) & (value < 0.5)) {
  myDiv.style.backgroundColor = "#ff4141";
} else if ((value < 1) & (value >= 0.5)) {
  myDiv.style.backgroundColor = "#ff6f6f";
} else if ((value < 1.5) & (value >= 1)) {
  myDiv.style.backgroundColor = "#ff6842";
} else if ((value < 2) & (value >= 1.5)) {
  myDiv.style.backgroundColor = "#ff8e42";
} else if ((value < 2.5) & (value >= 2)) {
  myDiv.style.backgroundColor = "#ffbd42";
} else if ((value < 3) & (value >= 2.5)) {
  myDiv.style.backgroundColor = "#ffe342";
} else if ((value < 3.5) & (value >= 3)) {
  myDiv.style.backgroundColor = "#dee943";
} else if ((value < 4) & (value >= 3.5)) {
  myDiv.style.backgroundColor = "#a1e943";
} else if ((value < 4.5) & (value >= 4)) {
  myDiv.style.backgroundColor = "#6ae943";
} else if ((value <= 5) & (value >= 4.5)) {
  myDiv.style.backgroundColor = "#37eb82";
}
