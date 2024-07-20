# AutoMat
A simple Unity3d editor script to create + set textures on HDRP lit materials, generate mask map from textures.<br />
The script expects the following: <br />
-all textures have read/write enabled <br />
-all textures use the following naming convention: <br />
name1_name2_name3_ttype <br />
where 'ttype' is one of the following: albedo,roughness,metallic,ao <br />
an example: <br />
TCom_AirDuct_1K_albedo.tif <br />
TCom_AirDuct_1K_ao.tif <br />
TCom_AirDuct_1K_normal.tif <br />
TCom_AirDuct_1K_roughness.tif <br />
TCom_AirDuct_1K_metallic.tif <br />
<br />
from the above source files the script creates a material named: AirDuct1K.mat <br />
then creates a mask named:  AirDuct1K_mask.tga  where R=Metallic,G=Ao,B=the detail mask will be empty,A = Smoothness (1-roughness) and assigns it as the material's mask texture; <br />
<br />
<br />
Usage:<br />
-Select ALL the textures you want to create materials+mask map for.<br />
-Run the script.<br />
