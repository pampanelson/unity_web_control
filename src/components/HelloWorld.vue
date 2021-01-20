<template>
	<h1>{{ msg }}</h1>
	<button @click="count++">count is: {{ count }}</button>
	<p>Edit <code>components/HelloWorld.vue</code> to test hot module replacement.</p>
</template>

<script>
	const GApi = 'global val api';
	export default {
		name: "HelloWorld",
		props: {
			msg: String
		},
		data() {
			return {
				count: 0
			}
		},
		beforeMount() {
			// console.log("before mounted");
		},
		mounted() {
			// console.log("mounted");
			console.log(this.$el);

		},
		created() {

		}
	}
</script>