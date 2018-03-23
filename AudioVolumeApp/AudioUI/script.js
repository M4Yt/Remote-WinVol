"use strict";

window.addEventListener("load", init);

let socket;
let slider, playpause, stop, prev, next, reconnect;

function init() {
    try {
        connectSocket();
    } catch(e) {
        openModal("connection-error");
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

    reconnect = document.getElementById("reconnect");
    reconnect.addEventListener("click", connectSocket);
}

function connectSocket() {
    let ip = location.hostname || "localhost";
    socket = new WebSocket("ws://"+ip+":8181");
    socket.onopen = () => {
        console.log("Connected!");
        closeModal("connection-error");
    }

    socket.onmessage = (e) => {
        setSliderPos(e.data);
    }

    socket.onclose = () => {
        openModal("connection-error");
    }
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

function openModal(id) {
    let modal = document.getElementById(id);
    modal.style.display = "flex";
    let content = modal.children[0];
    content.style.maxHeight = content.scrollHeight+"px";
}

function closeModal(id) {
    let modal = document.getElementById(id);
    let content = modal.children[0];
    modal.style.display = "none";
    content.style.maxHeight = "0";
}
