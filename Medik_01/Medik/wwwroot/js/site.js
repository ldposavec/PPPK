// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const circleElement = document.querySelector('.circle');

const mouse = { x: 0, y: 0 },
    circle = { x: 0, y: 0 };

window.addEventListener('mousemove', e => {
    mouse.x = e.x;
    mouse.y = e.y;
});

// Speed factor
// Between 0 and 1 (0 = smoother, 1 = instant)
const speed = 0.15;

const tick = () => {
    // (mouse.x - circle.x) calculates the difference between the current x-coordinate of the mouse and the current x-coordinate of the circle.
    // (mouse.x - circle.x) * speed multiplies the difference by the speed factor, which determines how quickly the circle should move towards the mouse position
    circle.x += (mouse.x - circle.x) * speed;
    circle.y += (mouse.y - circle.y) * speed;

    // Update circle element's position
    circleElement.style.transform = `translate(${circle.x}px, ${circle.y}px)`;

    // Call function on next frame
    window.requestAnimationFrame(tick);
}

tick();