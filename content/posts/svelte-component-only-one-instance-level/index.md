---
draft: false
date: 2024-10-11T13:22:42+11:00

author: lewis

title: How to have more than one script in a Svelte component
summary: Running into issues with adding more than one <script> tag in a Svelte component? This tutorial will show you how to fix it.

category: Tutorial

tags:
    - svelte
    - javascript
    - typescript
    - web

featured: ./static/featured.png
---

## A component can only have one instance-level \<script\> element
While you are learning how to use Svelte you may have run into this error and are scratching your head on what this even means.

When your component is created by Svelte (instantiated) it looks for the script area to execute. This happens on the server when a user requests the page for the first time (server-side rendering). Svelte requires that there is only one script tag in your component so there is no confusion on what script to run.

### But what if I want to include external scripts?
You can add the scripts to the page template (app.html), however what only want to include those scripts in a single page?

There is a solution for this, you can import a flag from the app environment that will tell you if you currently running in the browser context or the server context.

## Rendering in the Browser
To import the browser you need to add this import to your script section.
```ts
import { browser } from "$app/environment";
```

To use this flag you can wrap the script tags you want to include like this:
```html
{#if browser}
<script type="module" src="https://cyberbilby.com/static/mod.js"></script>
{/if}
```
This will solve the error by checking if the component is rendering in the browser context, then include the script in the page.