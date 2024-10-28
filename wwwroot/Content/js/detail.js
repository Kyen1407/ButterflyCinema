// Get the modal
var modal = document.getElementById("myModal");

// Get the button that opens the modal
var btn = document.getElementById("watchTrailer");

// Get the <span> element that closes the modal
var span = document.getElementsByClassName("close")[0];

// When the user clicks on the button, open the modal
btn.onclick = function() {
  modal.style.display = "block";
}

// When the user clicks on <span> (x), close the modal
span.onclick = function() {
	modal.style.display = "none";
	var iframe = document.getElementById('videoFrame');
	var iframeSrc = iframe.src;
	// Kiểm tra nếu iframe có nguồn từ YouTube
	if (iframeSrc.indexOf('youtube.com') !== -1) {
		// Gắn thêm "?autoplay=0" vào URL để tắt tự động phát
		iframe.src = iframeSrc.replace('autoplay=1', 'autoplay=0');
	}
}

// When the user clicks anywhere outside of the modal, close it
window.onclick = function(event) {
  	if (event.target == modal) {
		modal.style.display = "none";
		var iframe = document.getElementById('videoFrame');
		var iframeSrc = iframe.src;

		// Kiểm tra nếu iframe có nguồn từ YouTube
		if (iframeSrc.indexOf('youtube.com') !== -1) {
			// Gắn thêm "?autoplay=0" vào URL để tắt tự động phát
			iframe.src = iframeSrc.replace('autoplay=1', 'autoplay=0');
		}
	}
}

// var speed = 10; 
// var tab = document.getElementById("demo");
// var tab1 = document.getElementById("demo1");
// var tab2 = document.getElementById("demo2");
// tab2.innerHTML = tab1.innerHTML;
// function Marquee() {
//     if (tab2.offsetWidth - tab.scrollLeft <= 0)
//         tab.scrollLeft -= tab1.offsetWidth
//     else {
//         tab.scrollLeft++;
//     }
// }
// var MyMar = setInterval(Marquee, speed);
// tab.onmouseover = function() { clearInterval(MyMar) };
// tab.onmouseout = function() { MyMar = setInterval(Marquee, speed) };