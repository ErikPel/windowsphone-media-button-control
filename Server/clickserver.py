from flask import Flask, request
from pykeyboard import PyKeyboard
import easygui
import socket
from threading import Thread, Event
import os
app = Flask(__name__)
k = PyKeyboard()
port_to_use = 5000

@app.route("/volumeup")
def volumeup():
    k.tap_key(k.volume_up_key)
    return "VolumeUp"

@app.route("/volumedown")
def volumedown():
    k.tap_key(k.volume_down_key)
    return "VolumeDown"

@app.route("/volumemute")
def volumemute():
    k.tap_key(k.volume_mute_key)
    return "VolumeMute"

@app.route("/playpause")
def playpause():

    k.tap_key(k.media_play_pause_key)
    return "PlayPause"

@app.route("/nextsong")
def next():
    k.tap_key(k.media_next_track_key)
    return "NextSong"

@app.route("/previoussong")
def previous():
    k.tap_key(k.media_prev_track_key)
    return "PreviousSong"


class LoopThread(Thread):
    def __init__(self, stop_event, interrupt_event):
        s = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
        #We are just connecting to get our ip. Nothing to do with gmail. You can change this if you want
        s.connect(("gmail.com",80))
        self.host = s.getsockname()[0]
        s.close()
        self.stop_event = stop_event
        self.interrupt_event = interrupt_event
        Thread.__init__(self)

    def run(self):
        while not self.stop_event.is_set():
            self.loop_process()
            if self.interrupt_event.is_set():
                self.interrupted_process()
                self.interrupt_event.clear()

    def loop_process(self):
        if easygui.msgbox("Your Ip is " + self.host+":"+str(port_to_use),"Server Running", ok_button="Exit"):
                os._exit(0)

    def interrupted_process(self):
        print("Interrupted!")

STOP_EVENT = Event()
INTERRUPT_EVENT = Event()
thread = LoopThread(STOP_EVENT, INTERRUPT_EVENT)







if __name__ == '__main__':
    thread.start()
    app.run(host="0.0.0.0",port=port_to_use)
