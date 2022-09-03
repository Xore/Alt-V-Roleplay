<?php
/**
 * Open Source Social Network
 *
 * @packageOpen Source Social Network
 * @author    Open Social Website Core Team <info@informatikon.com>
 * @copyright 2014 iNFORMATIKON TECHNOLOGIES
 * @license   General Public Licence http://www.opensource-socialnetwork.org/licence
 * @link      http://www.opensource-socialnetwork.org/licence
 */
define('LoginSelector', ossn_route()->com . 'LoginSelector/');
function Login_Selector(){
		ossn_extend_view('index/ossn.default', 'pages/contents/index');
		ossn_extend_view('css/ossn.default', 'pages/home/css');
		
	}
ossn_register_callback('ossn', 'init', 'Login_Selector');