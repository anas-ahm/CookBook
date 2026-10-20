CREATE TABLE FoodItem (
                           id            INT AUTO_INCREMENT PRIMARY KEY,
                           name          VARCHAR(100) NOT NULL UNIQUE,
                           calories      DOUBLE NOT NULL DEFAULT 0,
                           protein       DOUBLE NOT NULL DEFAULT 0,
                           fat           DOUBLE NOT NULL DEFAULT 0,
                           saturated_fat DOUBLE NOT NULL DEFAULT 0,
                           carbs         DOUBLE NOT NULL DEFAULT 0,
                           sugar         DOUBLE NOT NULL DEFAULT 0,
                           fiber         DOUBLE NOT NULL DEFAULT 0,
                           salt          DOUBLE NOT NULL DEFAULT 0,

                           CONSTRAINT chk_food_item_calories      CHECK (calories >= 0),
                           CONSTRAINT chk_food_item_protein       CHECK (protein >= 0),
                           CONSTRAINT chk_food_item_fat           CHECK (fat >= 0),
                           CONSTRAINT chk_food_item_saturated_fat CHECK (saturated_fat >= 0 AND saturated_fat <= fat),
                           CONSTRAINT chk_food_item_carbs         CHECK (carbs >= 0),
                           CONSTRAINT chk_food_item_sugar         CHECK (sugar >= 0 AND sugar <= carbs),
                           CONSTRAINT chk_food_item_fiber         CHECK (fiber >= 0 AND fiber <= carbs),
                           CONSTRAINT chk_food_item_salt          CHECK (salt >= 0)
);