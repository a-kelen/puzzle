<template>
	<v-dialog model-value max-width="800">
		<v-card>
			<v-card-text class="pa-4 black--text text-h4">
				Your score: {{slides}} slides during {{time}}
				<Login />
			</v-card-text>
			<v-card-actions class="pt-3">
				<v-spacer></v-spacer>
				<v-btn
					color="grey"
					text
					class="body-2 font-weight-bold"
					@click="close"
				>
					Close & Reset
				</v-btn>
				<v-btn
					color="primary"
					class="body-2 font-weight-bold"
					outlined
					:disabled="!logged"
					@click="save"
				>Save</v-btn
				>
			</v-card-actions>
		</v-card>
	</v-dialog>
</template>

<script setup>
	import { ref, computed, useSlots } from 'vue';
	import Login from '@/components/Login.vue';
	import { useStore } from 'vuex';

	defineProps({
		slides: Number,
		time: String,
	});
	const emit = defineEmits(['close', 'save']);

	const store = useStore();
	const logged = computed(() => store.getters.logged);

	const dialog = ref(false);

	const close = () => {
		emit('close')
	};

	const save = () => {
		emit('save');
	};
</script>