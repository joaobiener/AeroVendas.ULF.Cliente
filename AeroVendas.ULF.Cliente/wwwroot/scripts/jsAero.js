
export function CopyClipbardSrcImage(IdElement) {
	// Get the text field
	var copySrc = document.getElementById(IdElement);

	// Select the text field
	//copySrc.select();
	//copySrc.setSelectionRange(0, 99999); // For mobile devices

	// Copy the text inside the text field
	navigator.clipboard.writeText(copySrc.src);


}