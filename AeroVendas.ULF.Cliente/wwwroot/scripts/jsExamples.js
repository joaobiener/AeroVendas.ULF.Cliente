﻿export function showAlert(message) {
	alert(message);
}

export function CopyClipbardSrcImage(IdElement) {
	// Get the text field
	var copySrc = document.getElementById(IdElement);

	// Select the text field
	copyText.select();
	copyText.setSelectionRange(0, 99999); // For mobile devices

	// Copy the text inside the text field
	navigator.clipboard.writeText(copyText.value);

	// Alert the copied text
	alert("Copied the text: " + copyText.value);
}
export function showAlertObject(person) {
	const message = 'This person is called ' + person.name + ' and is ' + person.age +
		' years old';

	alert(message);
}

export function emailRegistration(message) {
	const result = prompt(message);

	if (!result)
		return 'Please provide an email to register';

	const returnMessage = 'Thanks, your email: ' + result + ' has been registered.';

	return returnMessage;
}

export function splitEmailDetails(message) {
	const email = prompt(message);

	if (!email)
		return null;

	const firstPart = email.substring(0, email.indexOf("@"));
	const secondPart = email.substring(email.indexOf("@") + 1);

	return {
		'name': firstPart,
		'server': secondPart.split('.')[0],
		'domain': secondPart.split('.')[1]
	}
}

export function focusAndStyleElement(element) {
	element.style.color = 'red';
	element.focus();
}
export function focusInputComponent(id) {
	const element = document.getElementById(id);

	element.focus();
}


export function focusAndStyleInputComponent(id) {
	const element = document.getElementById(id);

	element.style.color = 'red';
	element.focus();
}

export function throwError() {
	throw Error("Testing error message from JS file.");
}