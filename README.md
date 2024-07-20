# AutoMat
A simple Unity3d editor script to create + set textures on HDRP lit materials, generate mask map from textures.
The script expects the following: 
-all textures have read/write enabled
-all textures use the following naming convention:
name1_name2_name3_ttype
where 'ttype' is one of the following: albedo,roughness,metallic,ao
an example:
TCom_AirDuct_1K_albedo.tif
TCom_AirDuct_1K_ao.tif
TCom_AirDuct_1K_normal.tif
TCom_AirDuct_1K_roughness.tif
TCom_AirDuct_1K_metallic.tif

from the above source files the script creates a material named: AirDuct1K.mat
then creates a mask named:  AirDuct1K_mask.tga  where R=Metallic,G=Ao,B=the detail mask will be empty,A = Smoothness (1-roughness) and assigns it as the material's mask texture;


Usage:
-Select ALL the textures you want to create materials+mask map for.
-Run the script.
