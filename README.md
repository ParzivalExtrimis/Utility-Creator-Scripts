# Utility-Creator-Scripts
Creator utility tools for ease of content building workflows. Create and update endpoints for content pipelines.

## Getting Started

* Clone the repository anywhere you'd like
* Make a new Unity Project version 2021.3.19f1 or use an existing project
  Note: using a different unity version may cause th ebuild to change scit building and may not behave as expected.
* Copy the files into a file named Editor in your Unity project root Assets/ folder. Make one if it doesn't exist
* You may need to reopen the project.
* You should now have a new tool bar option named bundles.

This allows you to build asset bundles for any assets in your project. For testing I would recommend using scenes. You wil find them in
Assets/Scenes/

## Building Asset Bundles

* Open the asset in the inspector window, in the bottom of the wondow you will findthe area to add labels to your asset bundles. 
* Add a label to the asset here, note that this will also be the name of your asset. Which will more or less have the structure of
<Label>-<PLatformName> 
* Make sure your labels are unique. Only one asset, in all the asets you have tagged can have a particular label.
* Once they are selected you can now go to the Bundles option in the tool bar and then click on Build.
* This should start a Unity builder process, once completed you should have a new folder named AssetBundles in your Assets/ directory with 
all your asset bundles in here.

## Meta files

* Once all your bundles have been generated to your satisfaction, you can now go back to the Bundles toolbar menu and select
the meta file generation option. This will open a window for the first bundle in your path that does not aleady have generated meta files.
* Enter the fields for you bundle.
* Once your done click on ok and this will launch a new window for the next asst bundle till all the bundles have meta files associated with them.
* You ca find these meta files in Assets/BundlesMeta/ 
* The Asset Bundle along with the meta.json file and unity meta files for each will together represent on unit of content and will be chunked together 
in pushes, pulls and runtime loading.
*The utilies for pushing onto the storage along with a feature enforcing admin consent for pushes. Along with a library utility making scripting the frontend
independent of the backend, allowing for effective decoupling of the two -- with  creator utility and library endpoints used to interact with the backend.
