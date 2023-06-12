import { createStore } from 'vuex';
import { isEqual } from 'lodash';
import { GRID_SIZE } from '@/constants';
import { shuffleArray, arrayToMatrix, parseJwt } from '@/utils';
import userService from './services/userService';
import api from './axios';
import scoreService from './services/scoreService';
import _ from 'lodash';
import moment from 'moment/moment';

export default createStore({
state () {
	return {
	loading: false,
	position: {
		x: 0,
		y: 0,
	},
	slides: 0,
	startTime: null,
	tiles: [],
	originalTiles: [],
	success: false,
	endTime: null,

	user: null,
	loginError: false,
	history: [],
	backup: null,
	}
},
getters: {
	logged: ({ user, loginError }) => () => {
		return !!user && !loginError;
	},
	bestScore: ({ history }) => () => {
	return _.minBy(history, function(x) {
		return x.slides;
	});
	}
},
mutations: {
	setLoading (state, payload) {
	state.loading = payload;
	},
	setPosition (state, payload) {
	state.position = payload;
	},
	setStartTime (state, payload) {
		state.startTime = payload;
	},
	initTiles (state) {
		const len = GRID_SIZE * GRID_SIZE - 1;
		let initArray = (new Array(len).fill(0)).map((_, i) => i + 1) ;
		state.originalTiles = arrayToMatrix([...initArray, 0], GRID_SIZE);

		// original code (comment this when testing end game)
		initArray = shuffleArray(initArray);
		initArray.push(0);
		state.tiles = arrayToMatrix(initArray, GRID_SIZE);
		state.position = {
		  x: GRID_SIZE - 1,
		  y: GRID_SIZE - 1,
		};
		// uncomment this for testing
		// state.tiles = arrayToMatrix([1,2,3,4,5,6,7,8,9,10,11,12,13,14, 0, 15], GRID_SIZE);
		// state.position = {
		// 	x: GRID_SIZE - 1,
		// 	y: GRID_SIZE - 2,
		// };
		state.startTime = Date.now();
	},
	resetResult (state) {
		state.success = false;
		state.slides = 0;
		state.endTime = null;
	},
	setTiles (state, payload) {
		state.tiles = payload;
	},
	setOriginalTiles (state, payload) {
		state.originalTiles = payload;
	},
	finish(state, payload) {
		state.success = payload;
		state.endTime = Date.now();
	},
	setSlides (state, payload) {
	state.slides = payload;
},
	incrementSlides (state) {
		state.slides++
	},
	swapTiles (state, { tileX, tileY }) {
		const { x, y } = state.position;
		const tiles = state.tiles;
		[tiles[x][y], tiles[tileX][tileY]] = [tiles[tileX][tileY], tiles[x][y]];
	},
	setUser(state, payload) {
	state.user = payload;
	state.loginError = false;
	},
	setLoginError(state, payload) {
	state.loginError = payload;
	},
	setHistory(state, payload) {
	state.history = payload;
	},
	addToHistory(state, payload) {
	state.history.unshift(payload);
	},
	setBackup(state, payload) {
	state.backup = payload;
	}
},
actions: {
	swapTiles (ctx, { x: tileX, y: tileY }) { 
		ctx.commit('swapTiles', { tileX, tileY });
		ctx.commit('setPosition', { x: tileX, y: tileY });
		ctx.commit('incrementSlides');
		const success = isEqual(ctx.state.tiles, ctx.state.originalTiles);
		if (success)
			ctx.commit('finish', success);
	},
	initTiles(ctx) {
		ctx.commit('initTiles');
	},
	resetTiles(ctx) {
		ctx.commit('initTiles');
		ctx.commit('resetResult');
	},
	async login({ commit }, { name, password }) {
	try {
		commit('setLoading', true);
		const { data } = await userService.login(name, password);
		commit('setUser', data);
		const token = data?.token;
		localStorage.setItem('token', token);
		api.defaults.headers.common.Authorization = `Bearer ${token}`;
		const tokenCredentials = parseJwt(token);
		const expire = tokenCredentials.exp * 1000 - Date.now();
		setTimeout(() => { commit('setUser', null) }, expire);
		commit('setHistory', data?.scores);
		commit('setBackup', data?.backup);
		commit('setLoading', false);
	} catch {
		commit('setLoginError', true);
		commit('setLoading', false);
	}
	},
	async current({ commit }) {
	try {
		commit('setLoading', true);
		const { data } = await userService.current();

		if (data) {
		commit('setUser', data);
		commit('setHistory', data?.scores);
		commit('setBackup', data?.backup);
		const token = localStorage.getItem('token');
		const tokenCredentials = parseJwt(token);
		const expire = tokenCredentials.exp * 1000 - Date.now();
		setTimeout(() => { commit('setUser', null) }, expire);
		commit('setLoading', false);
		}
	} catch {
		commit('setLoading', false);
	}
	},
	restoreFromBackup({ commit, dispatch, state }) {
	const { seconds, slides, tiles } = state.backup;
	const positionI = tiles.findIndex(x => x.value === 0);
	const startTime = moment().subtract(seconds, 'seconds').valueOf();
	const newTiles = arrayToMatrix(tiles.map(x => x.value), GRID_SIZE);
	
	commit('setTiles', newTiles);
	commit('setStartTime', startTime);
	commit('setSlides', slides);
	commit('setPosition', {
		x: Math.floor(positionI / GRID_SIZE),
		y: positionI % GRID_SIZE
	});
	dispatch('deleteBackup');
	},
	async saveBackup({ commit, state }) {
	try {
		commit('setLoading', true);
		const { slides, startTime, tiles } = state;
		const seconds = Math.floor((Date.now() - startTime) / 1000);
		const { data } = await userService.setBackup({
		slides,
		seconds,
		tiles: tiles.flat(),
		});
		commit('setBackup', data);
		commit('setLoading', false);
	} catch {
		commit('setLoginError', true);
		commit('setLoading', false);
	}
	},
	async deleteBackup({ commit }) {
	try {
		await userService.deleteBackup();
		commit('setBackup', null);
		commit('setLoading', false);
	} catch {
		commit('setLoading', false);
	}
	},
	async saveGame({ commit, state }) {
	try {
		commit('setLoading', true);
		const { slides, startTime, endTime } = state;
		const seconds = Math.floor((endTime - startTime) / 1000);
		const { data } = await scoreService.addScore({
		slides,
		seconds,
		});
		commit('addToHistory', data);
		commit('setLoading', false);
	} catch {
		commit('setLoginError', true);
		commit('setLoading', false);
	}
	},
}
})