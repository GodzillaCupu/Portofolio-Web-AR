document.getElementById('startARButton').click();

window.onmessage = (event) => {
  parent.postMessage({ isLoaded: true }, '*');
};
function handleScreenshotClose() {
  parent.postMessage({ refresh: true }, '*');
  location.reload(true);
  // if (counter === 4) {
  // } else {
  //   document.getElementById("screenshotDiv").style.display = "none";
  //   document.getElementById("screenshotImg").src = "";
  // }
}
function BackToWeb() {
  parent.postMessage({ redirectUrl: 'https://www.ancol.com/home' }, '*');
  console.log('Berhasil Kembali');
}
function SaveScreenshoot() {
  console.log('Save ScreenShoot ...');
  var imageSS = document.getElementById('screenshotImg').src;
  var downloadLink = document.createElement('a');
  downloadLink.href = imageSS;
  downloadLink.download = 'canvas_snapshot.png';
  document.body.appendChild(downloadLink);
  downloadLink.click();
}
function handleScreenshotClose() {
  parent.postMessage({ refresh: true }, '*');
  location.reload(true);
  // if (counter === 4) {
  // } else {
  //   document.getElementById("screenshotDiv").style.display = "none";
  //   document.getElementById("screenshotImg").src = "";
  // }
}
window.onmessage = (event) => {
  parent.postMessage({ isLoaded: true }, '*');
};
function handleScreenshotClose() {
  parent.postMessage({ refresh: true }, '*');
  location.reload(true);
}
function handleSendLoading(params) {
  parent.postMessage({ loading_progress: params }, '*');
}
function BackToWeb() {
  parent.postMessage({ redirectUrl: 'https://www.ancol.com/home' }, '*');
  console.log('Berhasil Kembali');
}
function SaveScreenshoot() {
  console.log('Save ScreenShoot ...');
  var imageSS = document.getElementById('screenshotImg').src;
  var downloadLink = document.createElement('a');
  downloadLink.href = imageSS;
  downloadLink.download = 'canvas_snapshot.png';
  document.body.appendChild(downloadLink);
  downloadLink.click();
}
function handleScreenshotClose() {
  parent.postMessage({ refresh: true }, '*');
  location.reload(true);
}

var isLandscape = false;
if (window.innerHeight > window.innerWidth) {
  console.log('potrait');
  isLandscape = false;
} else if (window.innerHeight < window.innerWidth) {
  console.log('landscape');
  isLandscape = true;
}
window.addEventListener('message', () => {
  if (event.data.isLandscape) {
    isLandscape = true;
  } else {
    isLandscape = false;
  }
});
// navigator.mediaDevices.getUserMedia({ video: true });
const isReady = () => {
  console.log('loaded frame');
  parent.postMessage({ isLoaded: true }, '*');
};
