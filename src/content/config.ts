import { defineCollection, z } from 'astro:content';

const blog = defineCollection({
	type: 'content',
	// Type-check frontmatter using a schema
	schema: z.object({
		title: z.string(),
		summary: z.string(),
        category: z.string(),
		// Transform string to Date object
		pubDate: z.coerce.date(),
        author: z.coerce.string(),
        contributors: z.array(z.string()).optional(),
		updatedDate: z.coerce.date().optional(),
		heroImage: z.string().optional(),
        hidden: z.boolean().optional()
	}),
});

const authors = defineCollection({
    type: 'data',

    schema: z.object({
        name: z.string()
    })
})

export const collections = { 
    blog, 
    authors 
};
