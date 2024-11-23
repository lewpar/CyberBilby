import { defineCollection, z } from 'astro:content';

const blog = defineCollection({
	type: 'content',
	// Type-check frontmatter using a schema
	schema: z.object({
		title: z.string(),
		summary: z.string(),
		// Transform string to Date object
		pubDate: z.coerce.date(),
        author: z.coerce.string(),
		updatedDate: z.coerce.date().optional(),
		heroImage: z.string().optional(),
	}),
});

const authors = defineCollection({
    type: 'data',

    schema: z.object({
        name: z.string(),
        avatar: z.string()
    })
})

export const collections = { 
    blog, 
    authors 
};
