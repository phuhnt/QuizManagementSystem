/**
 * @license Copyright (c) 2003-2017, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function( config ) {
	// Define changes to default configuration here. For example:
	// config.language = 'fr';
	// config.uiColor = '#AADC6E';

    config.extraPlugins = 'syntaxhighlight';
    config.syntaxhighlight_lang = 'csharp';
    config.syntaxhighlight_hideControls = true;
    confog.language = 'vi';
    config.filebrowserBrowserUrl = '';
    config.filebrowserImageBrowserUrl = '~/Assets/Admin/js/plugins/ckfinder.html?Type=Images';
    config.flashbrowserFlashBrowserUrl = '~/Assets/Admin/js/plugins/ckfinder.html?Type=Flash';
    config.filebrowserUploadUrl = '~/Assets/Admin/js/plugins/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Flash';
    config.filebrowserImageUploadUrl = '~/Data/';
    config.filebrowserFlashUploadUrl = '~/Assets/Admin/js/plugins/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Flash';
    CKFinder.setupCKEditor(null, '~/Assets/Admin/js/plugins/ckfinder/');
};