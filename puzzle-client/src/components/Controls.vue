<template>
  <v-container>
    <v-row class="align-center">
        <v-col cols="auto">
            <History />
        </v-col>
        <v-col cols="auto" class="me-auto">{{ prettyTime(time) }}</v-col>
        <v-col >
            <v-chip
                class="ma-2"
                color="orange"
                label
                text-color="white"
                >
                <v-icon start icon="mdi-drag-variant"></v-icon>
                {{ slides }}
            </v-chip>
            <v-chip
                v-show="logged"
                class="ma-2"
                color="blue"
                label
                >
                Logged
            </v-chip>
        </v-col>
        <v-col cols="auto">
            <v-btn variant="tonal" color="red">Reset  <ResetDialog  @confirm="handleReset" /></v-btn>
        </v-col>
        <v-col cols="auto">
            <v-btn variant="tonal" color="green">Save Backup <SaveBackup @confirm="handleSaveBackup" /></v-btn>
        </v-col>
        <v-col v-if="backup" cols="auto">
            <v-btn variant="tonal" color="pink-darken-3"><v-icon icon="mdi-autorenew">
                </v-icon>
                <RestoreBackupDialog @delete="handleDeleteBackup" @confirm="handleRestoreBackup" />
            </v-btn>
        </v-col>
    </v-row>          
  </v-container>
</template>

<script setup>
    import ResetDialog from '@/components/dialogs/ResetDialog.vue';
    import SaveBackup from '@/components/dialogs/SaveBackup.vue';
    import RestoreBackupDialog from '@/components/dialogs/RestoreBackupDialog.vue';
    import History from '@/components/History.vue';
    import { useStore } from 'vuex';
    import { computed, onMounted, watch, ref } from 'vue';
    import { prettyTime } from '@/utils';

    const time = ref(0);

    const store = useStore();
    const slides = computed(() => store.state.slides);
    const success = computed(() => store.state.success);
    const startTime = computed(() => store.state.startTime);
    const logged = computed(() => store.state.user);
    const backup = computed(() => store.state.backup);

    watch(startTime, (newStartTime) => {
        time.value = Math.floor((Math.abs(newStartTime - Date.now())) / 1000);
    }); 

    onMounted(() => {
        setInterval(() => {
            time.value++;
        }, 1000)
    });

    const handleReset = () => {
        store.dispatch('resetTiles');
    };

    const handleSaveBackup = () => {
        store.dispatch('saveBackup');
    };

    const handleDeleteBackup = () => {
        store.dispatch('deleteBackup');
    };

    const handleRestoreBackup = () => {
        store.dispatch('restoreFromBackup');
    };

</script>
