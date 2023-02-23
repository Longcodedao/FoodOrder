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

// const myDiv = document.querySelectorAll(".rating-color");
// const valueBox = document.querySelectorAll(".rating");

// for (let i = 0; i < myDivs.length; i++) {
//   const value = parseInt(valueBox[i].innerText);
//   if ((value >= 0) & (value < 0.5)) {
//     myDiv[i].style.backgroundColor = "#ff4141";
//   } else if ((value < 1) & (value >= 0.5)) {
//     myDiv[i].style.backgroundColor = "#ff6f6f";
//   } else if ((value < 1.5) & (value >= 1)) {
//     myDiv[i].style.backgroundColor = "#ff6842";
//   } else if ((value < 2) & (value >= 1.5)) {
//     myDiv[i].style.backgroundColor = "#ff8e42";
//   } else if ((value < 2.5) & (value >= 2)) {
//     myDiv[i].style.backgroundColor = "#ffbd42";
//   } else if ((value < 3) & (value >= 2.5)) {
//     myDiv[i].style.backgroundColor = "#ffe342";
//   } else if ((value < 3.5) & (value >= 3)) {
//     myDiv[i].style.backgroundColor = "#dee943";
//   } else if ((value < 4) & (value >= 3.5)) {
//     myDiv[i].style.backgroundColor = "#a1e943";
//   } else if ((value < 4.5) & (value >= 4)) {
//     myDiv[i].style.backgroundColor = "#6ae943";
//   } else if ((value <= 5) & (value >= 4.5)) {
//     myDiv[i].style.backgroundColor = "#37eb82";
//   }
// }

const myDivs = document.querySelectorAll(".rating-color");

for (let i = 0; i < myDivs.length; i++) {
  const value = parseInt(document.querySelectorAll(".rating").innerText);

  if (value < 0) {
    myDivs[i].style.backgroundColor = "red";
  } else if (value < 10) {
    myDivs[i].style.backgroundColor = "yellow";
  } else {
    myDivs[i].style.backgroundColor = "green";
  }
}
