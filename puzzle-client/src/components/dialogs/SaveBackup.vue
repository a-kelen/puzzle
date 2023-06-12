<template>
	<v-dialog
		v-model="dialog"
		activator="parent"
		@keydown.esc="cancel"
	>
		<v-card>
			<v-card-text class="pa-4 black--text">
				Are you sure you want to save the game?
				<Login />
			</v-card-text>
			<v-card-actions class="pt-3">
				<v-spacer></v-spacer>
				<v-btn
					color="grey"
					text
					class="body-2 font-weight-bold"
					@click="cancel"
				>
					Cancel
				</v-btn>
				<v-btn
					color="primary"
					class="body-2 font-weight-bold"
					outlined
					@click="confirm"
					:disabled="!logged"
				>
					Yes
				</v-btn>
			</v-card-actions>
		</v-card>
	</v-dialog>
</template>

<script setup>
	import { ref, computed } from 'vue';
	import Login from '@/components/Login.vue';
	import { useStore } from 'vuex';

	const emit = defineEmits(['confirm']);

	const store = useStore();
	const user = computed(() => store.state.user);
	const loginError = computed(() => store.state.loginError);
	const logged = computed(() => store.getters.logged);

	const dialog = ref(false);

	const cancel = () => {
		dialog.value = false;
	};

	const confirm = () => {
		emit('confirm');
		dialog.value = false;
	};
</script>