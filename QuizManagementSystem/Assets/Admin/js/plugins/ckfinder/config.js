/*
Copyright (c) 2003-2015, CKSource - Frederico Knabben. All rights reserved.
For licensing, see license.txt or http://cksource.com/ckfinder/license
*/

CKFinder.customConfig = function( config )
{
	// Define changes to default configuration here.
	// For the list of available options, check:
	// http://docs.cksource.com/ckfinder_2.x_api/symbols/CKFinder.config.html

	// Sample configuration options:
	// config.uiColor = '#BDE31E';
	// config.language = 'fr';
	// config.removePlugins = 'basket';
    config.language = 'vi';
    config.filebrowserBrowseUrl = "'" + location.hostname + "/Assets/Admin/js/plugins/ckfinder/ckfinder.html'";
    config.filebrowserImageBrowseUrl = "'" + location.hostname + "/Assets/Admin/js/plugins/ckfinder/ckfinder.html?type=Images'";
    config.filebrowserFlashBrowseUrl = "'" + location.hostname + "/Assets/Admin/js/plugins/ckfinder/ckfinder.html?type=Flash'";
    config.filebrowserUploadUrl = "'" + location.hostname + "/Assets/Admin/js/plugins/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Files'";
    config.filebrowserImageUploadUrl = "'" + location.hostname + "/Assets/Admin/js/plugins/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Images'";
    config.filebrowserFlashUploadUrl = "'" + location.hostname + "/Assets/Admin/js/plugins/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Flash'";
};
