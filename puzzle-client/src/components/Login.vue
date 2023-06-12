<template>
	<v-form v-if="!user" @submit.prevent class="pa-4">
	<v-text-field
		v-model="name"
		label="Name"
	></v-text-field>
	<v-text-field
		v-model="password"
		label="password"
		type="password"
	></v-text-field>
		<v-alert v-show="loginError" text="Invalid credentials" type="error" variant="tonal"></v-alert>
	<v-btn type="submit" @click="login" block color="blue" variant="outlined" class="mt-2">Login</v-btn>
	</v-form>
</template>

<script setup>
	import { ref, computed } from 'vue';
	import { useStore } from 'vuex';

	const name = ref('');
	const password = ref('');

	const store = useStore();
	const user = computed(() => store.state.user);
	const loginError = computed(() => store.state.loginError);

	const login = () => {
	store.dispatch('login', { name: name.value, password: password.value })
	};
</script>