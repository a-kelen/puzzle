<template>
	<v-sheet
		rounded
		class="d-flex align-center justify-center text-h4 ma-2 tile"
		:class="{ active: isActive, 'elevation-8': isActive }"
		height="100"
		width="100"
		:color="mappedColor"
		@click="click"
	>
	{{ mappedText }}
	</v-sheet>
</template>

<script setup>
	import { ref, computed } from 'vue';
	import { useStore } from 'vuex';
	import { GRID_SIZE } from '@/constants'
	
	const store = useStore();
	const position = computed(() => store.state.position);
	const tiles = computed(() => store.state.tiles);

	const { color, text, active } = defineProps({
		color: {
			type: String,
			required: true
		},
		text: {
			type: Number,
			required: true
		},
	
	});
	const emit = defineEmits(['onClick'])
	const isEmpty = text === 0;

	const isActive = computed(() => {
		const index = tiles.value.flat().findIndex(el => el === text);
		const x = Math.floor(index / GRID_SIZE)
		const y = index % GRID_SIZE;
		const { x: positionX, y: positionY } = position.value;
		const xDelta = Math.abs(x - positionX);
		const yDelta = Math.abs(y - positionY);

		return xDelta === 1 && yDelta === 0 || xDelta === 0 && yDelta === 1;
	}) 
	const mappedColor = isEmpty ? 'transparent' : color;
	const mappedText = isEmpty ? '' : text;

	const click = () => {
		if (!isEmpty && isActive.value) {
			emit('onClick', text);
		}
	}
</script>

<style lang="css" scoped>
	.tile {
		cursor: default;
		transition: .1s;
	}
	.active {
		cursor: pointer;
		font-weight: bolder;
		scale: 1.05;
}
</style>
