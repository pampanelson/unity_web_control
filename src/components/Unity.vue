<template>
    <div id="unity-container" class="unity-desktop">
      <canvas id="unity-canvas"
        @mouseover="setPause(0)"
        @mouseleave="setPause(1)" 
      ></canvas>
      <div id="unity-loading-bar">
        <div id="unity-logo"></div>
        <div id="unity-progress-bar-empty">
          <div id="unity-progress-bar-full"></div>
        </div>
      </div>
      <div id="unity-mobile-warning">
        WebGL builds are not supported on mobile devices.
      </div>
      <div id="unity-footer">
        <div id="unity-webgl-logo"></div>
        <div id="unity-fullscreen-button"></div>
        <div id="unity-build-title">TIM Demo</div>
      </div>
    </div>

    <!-- <button @click="debugBtn">debug unity button</button> -->
    <!-- <button @click="sendMsgBtn('GameObject','TestFunc','hello unity')">send msg to unity</button> -->
    <button @click="setMode(0)">轨道模式</button>
    <br>
    <br>
    <button @click="setMode(1)">漫游模式[ W A S D 控制前后左右移动]</button>
    <!-- <button @click="getCameraCurrentPosition()">Cam Target Position</button> -->
    <!-- <button @click="setMouseInput(0)">Disable Mouse</button> -->
    <!-- <button @click="setMouseInput(1)">Enable Mouse</button> -->
</template>

<script>
export default {
    name:"Unity",
    props:{

    },
    data(){
        return {
            unityInstance: null,
        }
    },
    beforeMount(){
    },
    methods:{
        debugBtn(){
            console.log("debug");
            if(this.unityInstance){
                // console.log(this.unityInstance);
            }
        },
        // sendMsgBtn(info){
        //   console.log(info);
        // },

        getCameraCurrentPosition(){
            this.message("MyCamera","getCameraCurrentPosition","");
        },

        sendMsgBtn(gameObject,method,param){
          this.message(gameObject,method,param);
          // console.log(gameObject);
        },

        // setMouseInput(val){
        //   this.message("FirstPersonCamera","SetInputCap",val);
        //   this.message("GameObject","setPause",val);
        // },
        setCamDistance(val){
          this.message("MyCamera","SetCameraDistance",val);
        },

        setMode(val){
          this.message("MyCamera","SetCameraMode",val);
        },
        
        setPause(val){
          console.log("hover " + val);
          this.message("MyCamera","SetCameraDisable",val);
          // this.message("GameObject","setPause",val);
        },
        message(gameObject, method, param) {
            if (param === null) {
                param = '';
            }
            if (this.unityInstance !== null){
                this.unityInstance.SendMessage(gameObject, method, param);
            } else {
                console.warn('vue-unity-webgl: you\'ve sent a message to the Unity content, but it wasn\t instantiated yet.');
            }
        },
    },

    mounted(){
      var container = document.querySelector("#unity-container");
        console.log(container);  

      var buildUrl = "Build";
      var loaderUrl = buildUrl + "/test.loader.js";

      var notCompressedConfig = {
        dataUrl: buildUrl + "/test.data",
        frameworkUrl: buildUrl + "/test.framework.js",
        codeUrl: buildUrl + "/test.wasm",
        streamingAssetsUrl: "StreamingAssets",
        companyName: "DefaultCompany",
        productName: "New Unity Project",
        productVersion: "0.1",
      };

      // config for compressed
      var compressedConfig = {
        dataUrl: buildUrl + "/test.data.unityweb",
        frameworkUrl: buildUrl + "/test.framework.js.unityweb",
        codeUrl: buildUrl + "/test.wasm.unityweb",
        streamingAssetsUrl: "StreamingAssets",
        companyName: "DefaultCompany",
        productName: "New Unity Project",
        productVersion: "0.1",
      };

      var config = notCompressedConfig;

      var container = document.querySelector("#unity-container");
      var canvas = document.querySelector("#unity-canvas");
      var loadingBar = document.querySelector("#unity-loading-bar");
      var progressBarFull = document.querySelector("#unity-progress-bar-full");
      var fullscreenButton = document.querySelector("#unity-fullscreen-button");
      var mobileWarning = document.querySelector("#unity-mobile-warning");

      if (/iPhone|iPad|iPod|Android/i.test(navigator.userAgent)) {
        container.className = "unity-mobile";
        config.devicePixelRatio = 1;
        mobileWarning.style.display = "none";
      } else {
        canvas.style.width = "960px";
        canvas.style.height = "600px";
      }
      loadingBar.style.display = "block";

      var script = document.createElement("script");
      script.src = loaderUrl;
      script.onload = () => {
        createUnityInstance(canvas, config, (progress) => {
          progressBarFull.style.width = 100 * progress + "%";
        }).then((unityInstance) => {

          this.unityInstance = unityInstance;
          loadingBar.style.display = "none";
          fullscreenButton.onclick = () => {
            unityInstance.SetFullscreen(1);
          };
        }).catch((message) => {
          alert(message);
        });
      };
      document.body.appendChild(script);



    }
}
</script>

<style>
</style>