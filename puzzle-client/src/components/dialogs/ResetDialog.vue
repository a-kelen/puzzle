<template>
    <v-dialog
        v-model="dialog"
        activator="parent"
        @keydown.esc="cancel"
    >
        <v-card>
            <v-card-text class="pa-4 black--text">
                Are you sure you want to reset the game?
            </v-card-text>
            <v-card-actions class="pt-3">
                <v-spacer></v-spacer>
                <v-btn
                color="grey"
                text
                class="body-2 font-weight-bold"
                @click="cancel"
                >Cancel</v-btn
                >
                <v-btn
                color="primary"
                class="body-2 font-weight-bold"
                outlined
                @click="confirm"
                >Yes</v-btn
                >
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>

<script setup>
    import { ref, computed } from 'vue';

    const emit = defineEmits(['confirm']);

    const dialog = ref(false);

    const cancel = () => {
        dialog.value = false;
    };

    const confirm = () => {
        emit('confirm');
        dialog.value = false;
    };
</script>