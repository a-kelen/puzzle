<template>
	<v-container>
		<v-btn icon="mdi-history" @click="drawer = !drawer"></v-btn>
		<v-navigation-drawer
			v-model="drawer"
			act
			temporary
			width="400"
		>
			<Login />
		<div v-if="logged">
			<div class="text-h5 pl-4 pt-4">History</div>
			<v-sheet v-if="bestScore" color="blue" rounded class="ma-3 py-2">
				<div class="pl-4 font-weight-bold">Best result</div>
				<v-list-item
						:title="moment(bestScore.created).format('MMMM Do YYYY, h:mm:ss')"
						:subtitle="`${bestScore.slides} slides during ${prettyTime(bestScore.seconds)}`"
				></v-list-item>
			</v-sheet>
			<v-list lines="one">
				<template  v-for="(item, i) in history"
					:key="item.id">
					<v-list-item
						:title="moment(item.created).format('MMMM Do YYYY, h:mm:ss')"
						:subtitle="`${item.slides} slides during ${prettyTime(item.seconds)}`"
					></v-list-item>
					<v-divider
						v-if="i < history.length - 1"
						:key="`${i}-divider`">
					</v-divider>
				</template>
			</v-list>
		</div>
	</v-navigation-drawer>
	</v-container>
</template>

<script setup>
	import moment from 'moment';
	import { ref, computed } from 'vue';
	import Login from '@/components/Login.vue';
	import { useStore } from 'vuex';
	import { prettyTime } from '@/utils';

	const emit = defineEmits(['confirm']);

	const store = useStore();
	const user = computed(() => store.state.user);
	const loginError = computed(() => store.state.loginError);
	const history = computed(() => store.state.history);
	const logged = computed(() => store.getters.logged());
	const bestScore = computed(() => store.getters.bestScore());

	const drawer = ref(false);

	const confirm = () => {
		emit('confirm');
		dialog.value = false;
	};
</script>