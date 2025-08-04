document.addEventListener('click', function (e) {
    createFlyingButterfly(e.pageX, e.pageY);
});

function createFlyingButterfly(x, y) {
    const butterfly = document.createElement('div');
    butterfly.innerHTML = '🦋';
    butterfly.style.position = 'fixed';
    butterfly.style.left = x + 'px';
    butterfly.style.top = y + 'px';
    butterfly.style.fontSize = '20px';
    butterfly.style.color = '#cf0000';
    butterfly.style.pointerEvents = 'none';
    butterfly.style.zIndex = '1000';
    butterfly.style.animation = 'flyAway 2s ease-out forwards';

    document.body.appendChild(butterfly);

    setTimeout(() => {
        butterfly.remove();
    }, 2000);
}

// Add CSS for flyAway animation
const style = document.createElement('style');
style.textContent = `
            @keyframes flyAway {
                0% {
                    transform: scale(1) rotate(0deg);
                    opacity: 1;
                }
                100% {
                    transform: scale(0.3) rotate(360deg) translateY(-200px);
                    opacity: 0;
                }
            }
        `;
document.head.appendChild(style);