"use strict";

window.addEventListener("load", init);

let socket;
let slider, playpause, stop, prev, next;

function init() {
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

    slider = document.getElementById("volume");
    slider.addEventListener("input", changeVolume);

    playpause = document.getElementById("playpause");
    playpause.addEventListener("click", sendMediaKey);

    stop = document.getElementById("stop");
    stop.addEventListener("click", sendMediaKey);

    prev = document.getElementById("prev");
    prev.addEventListener("click", sendMediaKey);

    next = document.getElementById("next");
    next.addEventListener("click", sendMediaKey);
}

function changeVolume() {
    let newVolume = this.value;
    socket.send(newVolume);
}

function setSliderPos(pos) {
    slider.value = pos;
}

function sendMediaKey() {
    socket.send(this.id);
}
