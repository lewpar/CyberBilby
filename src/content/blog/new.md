---
title: New Post
summary: Create a new Cyber Bilby post.
category: Tutorial
author: lewpar
pubDate: 2024-02-09T09:40:23+11:00
hidden: true
---

## Creating a new Cyber Bilby post
Cyber Bilby is an open source blogging platform for posting articles, tutorials, and blog posts. This means anybody can contribute to the website and make changes to existing posts.

By following this guide you can create your first post.

### Prerequisites

- **GitHub Account**

    Cyber Bilby uses GitHub as its content management system. Authors can submit posts through the pull-request system on GitHub and have it reviewed by myself. 
    
    As such, you need to [create a GitHub account](https://docs.github.com/en/get-started/start-your-journey/creating-an-account-on-github) to contribute.

- **Git**

    Although you can create and upload all of the files for the content through GitHub, its recommended you use a piece of version control software called Git to commit your changes to your forked clone.

    You can [download and install Git from here](https://git-scm.com/downloads).

- **Code Editor**

    To create and edit the Cyber Bilby source, you need a code editor to make your life easier. I recommend VSCode as it has very good integration with AstroJS & TypeScript.

    You can [download and install VSCode from here](https://code.visualstudio.com/).

### Fork & Clone Repository
To author content you need to first fork and clone the CyberBilby repository.
1. **Visit** https://github.com/lewpar/cyberbilby
2. **Click** `Fork` at the top right of the page
3. **Run** `git clone https://github.com/USERNAME/cyberbilby.git` in a location on your operating system where you would like to clone the repository. Make sure to replace `USERNAME` with your GitHub username.

### Opening and learning the codebase
To start, you should open the repository in your chosen code editor. If your chosen editor is VSCode you can follow these steps:
1. **Open VSCode**
2. **Click** `File` at the top left of the window
3. **Select** `Open Folder`
4. **Select the repository folder** you cloned earlier

Once you have the repository open you will see a bunch of different folders and files, I am only going to list the relevant folders that you will be touching to create a new post.
- public (Where static content is stored: images, stylesheets, etc..)
- src (Where the source code of Cyber Bilby and the posts reside)
    - content (Contains the posts and authors)
        - authors (The authors in YAML format)
        - blog (The posts in Markdown format)

### Creating your author profile
Before you can create a post you first need to setup your author profile. This is what links the posts to your identity.

Note: Anywhere in the following sub-sections that mention `USERNAME` must be replaced by your GitHub username in **_LOWERCASE_**.

#### Avatar
1. **Navigate** to `./public/avatars` in the codebase
2. **Create or add an image** that you would like to use in your profile with the format `USERNAME.png` (other formats than png is supported)

#### Profile
1. **Navigate** to `./src/content/authors` in the codebase.
2. **Create a new file** in the format `username.yaml`
3. **Open the new yaml file** you just created and populate it accordingly:
```yml
name: USERNAME
avatar: /avatars/USERNAME.png
```

Your author profile is now ready to be used for all your future posts.

### Creating your first post
1. **Navigate** to `./src/content/blog` in the codebase
2. **Create a new file** with the format `my-first-post.md`, ensuring the name is unique
    - URL slugs are the routes that are used to find blog posts in the address bar. An example of a URL slug is: `https://cyberbilby.com/blog/my-first-post` where `my-first-post` is the URL slug. 
3. **Open the file** you just created
4. **Populate the file** with the `front matter` like so:
    ```
    ---
    pubDate: 2024-08-14T12:30:42+11:00

    author: lewpar

    title: My first blog post
    summary: This is my first blog post on the Cyber Bilby website
    category: blog

    heroImage: /blog-content/my-first-post/hero.png
    ---

    MARKDOWN HERE
    ```
    Each of the posts use front matter, which is metadata at the start of each post where you set things like the name, image, author, and more for the post. 

    This front matter is encapsulated with three dashes (`---`) to signify where the front matter starts and ends. It is not optional.

    I will explain each of the front matter options available:
    - **pubDate** (Required)

        This is the publish date of your post, it is always in ISO 8601 format.
        If you don't want to write your own datetime you can [use an online converter](https://www.timestamp-converter.com/)

    - **author** (Required)

        This is the name of the author / GitHub username that was created earlier. This sets the author that should show on the post.

    - **title** (Required)

        This sets the title of the post, which will show up on the blog index and post.

    - **summary** (Required)

        This is a very short (50-150) summary of what the post is about. This is only shown on the blog index.

    - **category** (Required)

        This sets the category for the post, so readers know what type of post it is. This is shown on the blog index.

    - **heroImage** (Optional)

        This sets the image that shows on the blog post and blog index. This is optional but recommended.

        The image size for this should be `512` pixels wide and `256` pixels high (`512x256`) to keep the file size low and consistent with other images.

        The next section explains how to setup the hero image.
5. **Populate your post** with markdown content (you can use [common mark](https://commonmark.org/) as a cheatsheet), replacing `MARKDOWN HERE`

### Adding static content to your post
Like mentioned in the previous section, it's recommended to have a hero image for your post.
You may also want to have other static images in your post, so you need an area where you can upload these files that is separated from other posts.

1. Navigate to `./public/blog-content`
2. Create a folder with your blog post slug like so: `./public/blog-content/my-first-post`
3. Move your hero image or static assets here

### Commit, push and merge your changes
I will now walk you through committing your changes to a local repository, pushing those changes to your remote GitHub repository, and opening a pull request on the Cyber Bilby official repository.

#### Commit
1. **Open your terminal** and navigate to your cloned repository:
   ```bash
   cd /path/to/cyberbilby
   ```
2. **Check the status** of your repository to view untracked/modified files:
   ```bash
   git status
   ```
3. **Stage your changes** for commit:
   ```bash
   git add .
   ```
   - This stages all modified and new files. Alternatively, you can add specific files:
     ```bash
     git add ./src/content/blog/my-first-post.md
     git add ./public/blog-content/my-first-post/
     ```
4. **Commit your changes** with a meaningful message:
   ```bash
   git commit -m "Add my-first-post blog entry"
   ```

#### Push
1. Push your changes to your forked repository on GitHub:
   ```bash
   git push
   ```

#### Merge
1. **Go to your forked repository** on GitHub. 
   - URL: `https://github.com/USERNAME/cyberbilby`
2. You should see a prompt: *"Compare & pull request"*—click this button
   - If you don’t see the prompt, click **Pull Requests** in the top menu and then click **New pull request**
3. **Set the base repository** to the original Cyber Bilby repository:
   - `base repository: lewpar/cyberbilby`
   - `base branch: master`
4. Add a **title and description** for your pull request:
   - Example title: *"Add new post: My First Post"*
   - Example description: *"This PR adds a blog post titled 'My First Post'."*
5. Click **Create Pull Request**

### Review
Your pull request is now submitted. At this point, all you need to do is wait for me to review it. Once it's approved, your changes will be merged into the Cyber Bilby repository, and your post will go live. 