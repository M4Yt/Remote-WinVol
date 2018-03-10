"use strict";

window.addEventListener("load", init);

let slider;
let socket;

function init() {
    slider = document.getElementById("volume");
    slider.addEventListener("input", changeVolume);

    let ip = location.hostname || "localhost";
    socket = new WebSocket("ws://"+ip+":8181");
    socket.onopen = () => {
        console.log("Connected!");
    }

    socket.onmessage = (e) => {
        setSliderPos(e.data);
    }

    socket.onclose = () => {
        slider.setAttribute("disabled","");
    }
}

function changeVolume() {
    let newVolume = this.value;
    socket.send(newVolume);
}

function setSliderPos(pos) {
    slider.value = pos;
}
