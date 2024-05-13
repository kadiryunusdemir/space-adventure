mergeInto(LibraryManager.library, {
  StartVideoRecording: function () {
    startCamera();
  },
  StopVideoRecording: function (str) {
    levelData =UTF8ToString(str);
    stopRecording(levelData);
  },
});
