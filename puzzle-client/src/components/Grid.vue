<template>
  <v-container class="d-flex">
    <v-responsive class="py-6">
        <v-row class="d-flex justify-center" v-for="(row, i) in tiles" :key="i">
            <Tile 
                v-for="tile in row"
                :key="tile"
                :color="color" 
                :text="tile" 
                @on-click="handleTileClick" />
        </v-row>
    </v-responsive>
  </v-container>
</template>

<script setup>
    import Tile from '@/components/Tile.vue';
    import { TILE_COLORS, GRID_SIZE } from '@/constants';
    import { ref, reactive, onMounted, computed } from 'vue';
    import { useStore } from 'vuex';
    import _ from 'lodash';

    const color = ref(TILE_COLORS[Math.floor(Math.random() * TILE_COLORS.length)]);

    const store = useStore();
    const tiles = computed(() => store.state.tiles);
    const position = computed(() => store.state.position);
    
    const handleTileClick = (text) => {
        const index = tiles.value.flat().findIndex(el => el === text);
        store.dispatch('swapTiles', { x: Math.floor(index / GRID_SIZE), y: index % GRID_SIZE });
    }

    onMounted(() => {
        store.dispatch('initTiles');
    });

    
</script>
