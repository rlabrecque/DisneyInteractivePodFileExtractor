DisneyInteractivePodFileExtractor
======

This is an extractor for the .pod files in [Sanctuary Woods Multimedia Corporation](http://www.mobygames.com/company/sanctuary-woods-inc)/Disney Interactive Victoria Studio games released in the 1990s.

So far this has only been tested with [Disney's Tarzan Activity Center](http://www.mobygames.com/game/disneys-activity-centre-tarzan).

The file format is extremely simple. It's just a bundle of files, no compression, checksums or built in offsets.

Towards the end of writing this I realized that there are actually 3 different types of .pod files. You can differentiate between them by their file signature.

This application only supports "Pod\0file\0\0\0\0" which are most common in the "X:\setup\Assets" folder on the CD. "Pod" and "Pod File\0\0\0\0" .pod files are found in the installation directory and often come out of extracting the pod files from the CD. I believe the other two types of files may be sprite information.
