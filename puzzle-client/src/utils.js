export function shuffleArray(inputArray) {
	const array = [...inputArray];
	for (let i = array.length - 1; i > 0; i--) {
		const j = Math.floor(Math.random() * (i + 1));
		[array[i], array[j]] = [array[j], array[i]];
	}

	return array;
}

export function arrayToMatrix(array, size) {
	const matrix = [];
	for (let i = 0; i < array.length; i += size) {
		matrix.push(array.slice(i, i + size));
	}

	return matrix;
}

export function prettyTime (time) {
	const seconds = time % 60;
	const minutes = Math.floor(time / 60);
	const hours = (Math.floor(time / 3600));
	
	
	return new Date(1, 1, 1, hours, minutes, seconds).toLocaleTimeString();
}

export function parseJwt (token) {
	try {
	return JSON.parse(atob(token.split('.')[1]));
	} catch (e) {
	return null;
	}
}