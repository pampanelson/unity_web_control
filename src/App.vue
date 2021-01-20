<template>
  <img alt="Vue logo" src="./assets/logo.png" />
  <HelloWorld msg=" Vue 3.0 + Vite" />
  <Unity/>
</template>

<script>
import HelloWorld from "./components/HelloWorld.vue"
import Unity from "./components/Unity.vue"

export default {
  name: "App",
  components: {
    HelloWorld,
    Unity
  }
}
</script>
