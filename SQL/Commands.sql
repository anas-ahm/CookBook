
ALTER TABLE FoodItem
    ADD COLUMN category VARCHAR(30) NOT NULL DEFAULT 'Other' AFTER name;


ALTER TABLE FoodItem
    ADD CONSTRAINT chk_food_item_category CHECK (category IN (
                                                              'Vegetable', 'Fruit', 'Meat', 'Fish', 'PlantProtein', 'DairyAndEggs',
                                                              'Carbs', 'NutsAndSeeds', 'FatsAndOils', 'Spices', 'Condiment',
                                                              'Sweets', 'Beverage', 'Other'
        ));