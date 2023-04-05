function ChangeImage(UploadImage, previewImg1, previewImg2, previewImg3) {
    if (UploadImage.files && UploadImage.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $(previewImg1).attr('src', e.target.result);
            $(previewImg2).attr('src', e.target.result);
            $(previewImg3).attr('src', e.target.result);
        }
        reader.readAsDataURL(UploadImage.files[0]);
    }
}
