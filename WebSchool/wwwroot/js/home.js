const prevButton = document.getElementById('prev');
const nextButton = document.getElementById('next');

const firstImage = document.getElementById('first');
const secondImage = document.getElementById('second');
const thirdImage = document.getElementById('third');
const fourthImage = document.getElementById('fourth');

let currentIndex = 0;
const images = [firstImage, secondImage, thirdImage, fourthImage];

prevButton.addEventListener('click', () => {
    const oldIndex = currentIndex;
    images[oldIndex].style.opacity = 0;

    if (currentIndex === 0) {
        currentIndex = images.length - 1;
    }
    else {
        currentIndex--;
    }

    setTimeout(() => {
        images[oldIndex].style.display = 'none';
        images[currentIndex].style.opacity = 1;
        images[currentIndex].style.display = 'block';
    }, 1000);
});

nextButton.addEventListener('click', () => {
    const oldIndex = currentIndex;
    images[oldIndex].style.opacity = 0;

    if (currentIndex === images.length - 1) {
        currentIndex = 0;
    }
    else {
        currentIndex++;
    }

    setTimeout(() => {
        images[oldIndex].style.display = 'none';
        images[currentIndex].style.opacity = 1;
        images[currentIndex].style.display = 'block';
    }, 1000);
});

setInterval(() => {
    const oldIndex = currentIndex;
    images[oldIndex].style.opacity = 0;

    if (currentIndex === images.length - 1) {
        currentIndex = 0;
    }
    else {
        currentIndex++;
    }

    setTimeout(() => {
        images[oldIndex].style.display = 'none';
        images[currentIndex].style.opacity = 1;
        images[currentIndex].style.display = 'block';
    }, 1000);
}, 5000);