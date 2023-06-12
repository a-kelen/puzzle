<template>
	<v-app>
		<v-main>
			<Controls />
			<Grid />
			<SuccessDialog v-if="successDialog" :slides="slides" :time="time" @close="handleClose" @save="handleSave" />
			<v-overlay
				:model-value="loading"
				class="align-center justify-center"
				persistent
			>
				<v-progress-circular
				color="blue-grey-darken-2"
				indeterminate
				size="64"
				></v-progress-circular>
			</v-overlay>
		</v-main>
	</v-app>
</template>

<script setup>
	import Controls from '@/components/Controls.vue';
	import Grid from '@/components/Grid.vue';
	import SuccessDialog from '@/components/dialogs/SuccessDialog.vue';
	import { ref, watch, computed, onMounted } from 'vue';
	import { useStore } from 'vuex';
	import { prettyTime } from '@/utils';
	
	const successDialog = ref(false);

	const store = useStore();
	const success = computed(() => store.state.success);
	const loading = computed(() => store.state.loading);
	const startTime = computed(() => store.state.startTime);
	const endTime = computed(() => store.state.endTime);
	const slides = computed(() => store.state.slides);
	const time = computed(() => prettyTime(Math.fround((endTime.value - startTime.value) / 1000)));

	watch(success, () => {
		if(success.value) {
		successDialog.value = true;
		}
	});

	onMounted(() => {
		store.dispatch('current');
	});

	const handleClose = () => {
		successDialog.value = false;
		store.dispatch('resetTiles');
	};
	const handleSave = () => {
		successDialog.value = false;
		store.dispatch('saveGame');
	};

</script>
